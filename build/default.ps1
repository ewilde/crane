properties{
    $configuration = "Debug"
    $build_number = 0
    [switch]$teamcityBuild = $false
    $chocolateyApiKey = ""
    $chocolateyApiUrl = ""
    [switch]$verbose = $false
}

$build_dir = (Split-Path $psake.build_script_file)
$root_dir =  Resolve-Path "$build_dir\.."
$build_artifacts_dir = "$root_dir\build-output"
$src_dir = "$root_dir\src"
$template_source_dir = "$src_dir\Crane.Templates"
$sln_filename = "Crane.sln"
$sln_filepath = "$src_dir\$sln_filename"
$xunit_consoleRunner = "$src_dir\packages\xunit.runners.1.9.2\tools\xunit.console.clr4.exe"
$version = ""

$add_includes = Join-Path $build_dir "add-includes.ps1"

& $add_includes @($build_dir)


FormatTaskName (("-"*25) + "[{0}]" + ("-"*25))

Task Default -Depends SetupContext, Clean, Build
<#
Task TeamCityBuildStep -Depends PatchAssemblyInfo, BuildCrane, Test, ChocolateyPublishPackage
Task Default -Depends BuildCrane, Test

Task BuildCrane -Depends Info, Clean, Build
#>
Task SetupContext {
  $global:context = ContextClass -psake_build_script_dir $build_dir -relative_solution_path "..\src\crane.sln" -configuration $configuration -build_number $build_number
  $global:context
}

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
  Write-Host verbose: $verbose
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
    $choco_output_dir = "$root_dir\chocolatey-output"
    $choco_nuspec = "$choco_output_dir\crane.nuspec"

    Remove-Item $choco_output_dir -Recurse -Force -ErrorAction SilentlyContinue
    New-Item -ItemType directory -Path $choco_output_dir -Force

    $nuspectemplate = Get-Content "$src_dir\Crane.Chocolatey\crane.nuspec" | Out-String
    $nuspectemplate = $nuspectemplate.Replace("##version_number##", "$(Get-Content -Path "$root_dir\VERSION.txt").$build_number")
    $nuspectemplate = $nuspectemplate.Replace("##build_output##", $build_artifacts_dir)


    New-Item -Path $choco_nuspec -ItemType File -Value $nuspectemplate
    & cpack @($choco_nuspec)

    Move-Item *.nupkg $choco_output_dir
}

Task ChocolateyPublishPackage -Depends ChocolateyBuildPackage{
    Get-ChildItem "$root_dir\chocolatey-output" -Filter *.nupkg |
    Foreach-Object{
        & $build_dir\nuget.exe @('push', $_.FullName, "-s", $chocolateyApiUrl, $chocolateyApiKey)
    }
}

Task PatchAssemblyInfo {
    $version = "$(Get-Content -Path "$root_dir\VERSION.txt").$build_number"
    GenerateAssemblyInfo "Crane.Core" "Core crane functionality" $version "$src_dir\crane.core\Properties\AssemblyInfo.cs"

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
    Invoke-GenerateAssemblyInfo -title $title -description $description -version $version -file $file
}

function Debug($message)
{
    Write-Verbose $message
}
