properties{
    $configuration = "Debug"
    $build_number = 0
    [switch]$teamcityBuild = $false
    $chocolateyApiKey = ""
    $chocolateyApiUrl = ""
}

$build_dir = (Split-Path $psake.build_script_file)
$root_dir =  Resolve-Path "$build_dir\.."
$isGitRepo = Test-Path (Join-Path $root_dir '.git')
$build_artifacts_dir = "$root_dir\build-output"
$src_dir = "$root_dir\src"
$sln_filename = "%context.ProjectName%.sln"
$sln_filepath = "$src_dir\$sln_filename" 
$xunit_consoleRunner = "$src_dir\packages\xunit.runners.1.9.2\tools\xunit.console.clr4.exe"
$version = ""

Import-Module (Join-Path $build_dir 'psake-ext.psm1')
FormatTaskName (("-"*25) + "[{0}]" + ("-"*25))

Task Default -Depends PatchAssemblyInfo, BuildCore, Test

Task BuildCore -Depends Info, Clean, Build

Task Info {
  Write-Host build_dir: $build_dir
  Write-Host build_artifacts_dir: $build_artifacts_dir
  Write-Host build_number: $build_number
  Write-Host configuration: $configuration
  Write-Host teamcityBuild: $teamcityBuild
  Write-Host src_dir: $src_dir
  Write-Host sln_filename: $sln_filename
  Write-Host sln_filepath: $sln_filepath
  Write-Host xunit_consoleRunner: $xunit_consoleRunner
}

Task Build -Depends Clean, NugetRestore { 
    Write-Host "Building $sln_filename ($configuration)" -ForegroundColor Green
    Exec { msbuild "$sln_filepath" /t:ReBuild /p:Configuration=$configuration /v:quiet /p:OutDir=$build_artifacts_dir } 
}

Task Clean {
    Write-Host "Creating build-output directory" -ForegroundColor Green
    if (Test-Path $build_artifacts_dir) 
    {   
        rd $build_artifacts_dir -rec -force | out-null
    }
    
    mkdir $build_artifacts_dir | out-null
    
    Write-Host "Cleaning $sln_filename ($configuration)" -ForegroundColor Green
    Exec { msbuild $sln_filepath /t:Clean /p:Configuration=$configuration /v:quiet } 
}

Task NugetRestore -Depends NugetExists { 
   & $build_dir\nuget.exe @('restore', $sln_filepath)
}

Task NugetExists { 
    Invoke-DownloadNuget $build_dir #doesn't download if exists
}

Task Test {
    Get-ChildItem -Path $build_artifacts_dir -Filter *.UnitTests.dll | 
    % {
        & $xunit_consoleRunner @($_.FullName, '/silent')
        if($LastExitCode -ne 0)
        {
          throw "failed executing tests $_.FullName. See last error."
        }
    }
}

Task PatchAssemblyInfo {
  $version = "$(Get-Content -Path "$root_dir\VERSION.txt").$build_number"
  GenerateAssemblyInfo "%context.ProjectName%" "%context.ProjectName% functionality" $version "$src_dir\%context.ProjectName%\Properties\AssemblyInfo.cs"
  GenerateAssemblyInfo "%context.ProjectName%.UnitTests" "%context.ProjectName%.UnitTests test library" $version "$src_dir\%context.ProjectName%.UnitTests\Properties\AssemblyInfo.cs"
	
  if ($teamcityBuild) {
		Write-Host "##teamcity[buildNumber '$version']"
	}
}

function GenerateAssemblyInfo
{
param(
	[string]$title, 
	[string]$description, 
	[string]$version,
	[string]$file
)
    $commit = ''
    if ($isGitRepo)
    {
        $commit = Get-Git-CommitMessage
    }

    Invoke-GenerateAssemblyInfo -title $title -description $description -version $version -file $file -commitMessage $commit
}
