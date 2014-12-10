properties{
    $configuration = "Debug"
    $build_number = 0
	[switch]$teamcityBuild = $false
}

$build_dir = (Split-Path $psake.build_script_file)
$root_dir = "$build_dir\.."
$build_artifacts_dir = "$build_dir\..\build-output"
$src_dir = "$root_dir\src"
$template_source_dir = "$src_dir\Crane.Templates"
$sln_filename = "Crane.sln"
$sln_filepath = "$src_dir\$sln_filename" 
$xunit_consoleRunner = "$src_dir\packages\xunit.runners.1.9.2\tools\xunit.console.clr4.exe"
  

Import-Module (Join-Path $build_dir 'psake-ext.psm1') -Force
FormatTaskName (("-"*25) + "[{0}]" + ("-"*25))

Task TeamCityBuildStep -Depends PatchAssemblyInfo, BuildCrane, Test 
Task Default -Depends BuildCrane, Test

Task BuildCrane -Depends Info, Clean, Build

Task Info {
  Write-Host build_dir: $build_dir
  Write-Host build_artifacts_dir: $build_artifacts_dir
  Write-Host build_number: $build_number
  Write-Host configuration: $configuration
  Write-Host teamcityBuild: $teamcityBuild
  Write-Host src_dir: $src_dir
  Write-Host template_source_dir: $template_source_dir
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
    Get-ChildItem -Path $build_artifacts_dir -Filter *.Tests.dll | 
    % {
        Debug("$xunit_consoleRunner @($($_.FullName), '/silent')")
        & $xunit_consoleRunner @($_.FullName, '/silent')
        if($LastExitCode -ne 0)
        {
          throw "failed executing tests $_.FullName. See last error."
        }
    }
}

Task ChocolateyExists{
    try{
	    & choco 
    }catch{	    
        iex ((new-object net.webclient).DownloadString('https://chocolatey.org/install.ps1'))
    }
}

Task ChocolateyBuildPackage -Depends ChocolateyExists{
    $choco_output_dir = Resolve-Path "$src_dir\..\chocolatey-output"
    $choco_nuspec = "$choco_output_dir\crane.nuspec"

    Remove-Item $choco_output_dir -Recurse -Force -ErrorAction SilentlyContinue
    New-Item -ItemType directory -Path $choco_output_dir -Force

    $template = Get-Content "$src_dir\Crane.Chocolatey\crane.nuspec" | Out-String
    $template = $template.Replace("##version_number##", $version)
    $template = $template.Replace("##build_output##", $build_artifacts_dir)
                     

    New-Item -Path $choco_nuspec -ItemType File -Value $template
    & cpack @($choco_nuspec)

    Move-Item *.nupkg $choco_output_dir
}

Task ChocolateyPublishPackage -Depends ChocolateyBuildPackage{
    Get-ChildItem $choco_output_dir -Filter *.nupkg | `
    Foreach-Object{
        & $build_dir\nuget.exe @('push', $_.FullName, "-s", "http://chocolatey.cranebuild.com:8080/", $ChocolateyApiKey)
    }
}

Task PatchAssemblyInfo {
    $version = "$(Get-Content -Path "$root_dir\VERSION.txt").$build_number"
    GenerateAssemblyInfo "Crane.Core" "Core crane functionality" $version "$src_dir\crane.core\Properties\AssemblyInfo.cs"

	if ($teamcityBuild) {
		[System.Console]::WriteLine("##teamcity[crane.buildnumber '$version']")
        [System.Console]::WriteLine("##teamcity['crane.buildnumber' '$version']")
        [System.Console]::WriteLine("##teamcity[setParameter name='crane.buildnumber' value='$version']")
        Write-Host "##teamcity[crane.buildnumber '$version']"
        Write-Host "##teamcity['crane.buildnumber' '$version']"
        Write-Host "##teamcity[setParameter name='crane.buildnumber' value='$version']"
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
    Invoke-GenerateAssemblyInfo -title $title -description $description -version $version -file $file
}

function Debug($message)
{
    Write-Verbose $message
}