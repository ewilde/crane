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


Task TeamCityBuildStep -Depends PatchAssemblyInfo, BuildCrane, Test, ChocolateyPublishPackage
Task Default -Depends BuildCrane, Test

Task BuildCrane -Depends SetupContext, Clean, Build


Task SetupContext {
  $global:context = ContextClass -psake_build_script_dir $build_dir -relative_solution_path "..\src\crane.sln" -configuration $configuration -build_number $build_number
  $global:context
}


Task NugetExists {
  Invoke-DownloadNuget "$($global:context.build_dir)"
}


Task ChocolateyExists{
    try{
	    & choco
    }catch{
        iex ((new-object net.webclient).DownloadString('https://chocolatey.org/install.ps1'))
    }
}

Task ChocolateyBuildPackage -Depends ChocolateyExists{
    $choco_output_dir = "$($global:context.root_dir)\chocolatey-output"
    $choco_nuspec = "$choco_output_dir\crane.nuspec"

    Remove-Item $choco_output_dir -Recurse -Force -ErrorAction SilentlyContinue
    New-Item -ItemType directory -Path $choco_output_dir -Force

    $nuspectemplate = Get-Content "$($global:context.root_dir)\src\Crane.Chocolatey\crane.nuspec" | Out-String
    $nuspectemplate = $nuspectemplate.Replace("##version_number##", $($global:context.build_version))
    $nuspectemplate = $nuspectemplate.Replace("##build_output##", $($global:context.build_artifacts_dir))


    New-Item -Path $choco_nuspec -ItemType File -Value $nuspectemplate
    & cpack @($choco_nuspec)

    Move-Item *.nupkg $choco_output_dir
}

Task ChocolateyPublishPackage -Depends ChocolateyBuildPackage{
    Get-ChildItem "$($global:context.root_dir)\chocolatey-output" -Filter *.nupkg |
    Foreach-Object{
        & $build_dir\nuget.exe @('push', $_.FullName, "-s", $chocolateyApiUrl, $chocolateyApiKey)
    }
}

Task PatchAssemblyInfo {
    $version = $global:context.build_version
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
