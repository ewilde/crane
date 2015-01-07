properties{
    $configuration = "Debug"
    $build_number = 0
    [switch]$teamcityBuild = $false
    $chocolateyApiKey = ""
    $chocolateyApiUrl = ""
    [switch]$verbose = $false
}

$build_dir = (Split-Path $psake.build_script_file)
$add_includes = Join-Path $build_dir "add-includes.ps1"

& $add_includes @($build_dir)


FormatTaskName (("-"*25) + "[{0}]" + ("-"*25))

Task TeamCityBuildStep -Depends SetupContext, PatchAssemblyInfo, BuildCrane, Test, ChocolateyPublishPackage
Task Default -Depends SetupContext, BuildCrane, Test
Task BuildCrane -Depends Clean, Build


Task SetupContext {
  $global:context = ContextClass -psake_build_script_dir $build_dir -relative_solution_path "..\src\crane.sln" -configuration $configuration -build_number $build_number
  $global:context
}


Task NugetExists {
  Invoke-DownloadNuget "$($global:context.build_dir)"
}


Task PatchAssemblyInfo {
    $version = $global:context.build_version
    GenerateAssemblyInfo "Crane.Core" "Core crane functionality" $version "$($global:context.sln_file_info.Directory.FullName)\src\crane.core\Properties\AssemblyInfo.cs"

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
