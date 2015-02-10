


Task Clean -Depends SetupContext {
  Write-Host "Creating build-output directory" -ForegroundColor Green
  if (Test-Path $($global:context.build_artifacts_dir)){
    rd $($global:context.build_artifacts_dir) -rec -force | out-null
  }
  sdf - sd

  mkdir $($global:context.build_artifacts_dir) | out-null

  Write-Host "Cleaning $($global:context.sln_file_info.Name) ($($global:context.configuration))" -ForegroundColor Green
  Exec { msbuild $($global:context.sln_file_info.FullName) /t:Clean /p:Configuration=$($global:context.configuration) /v:quiet }
}


Task NugetRestore -Depends SetupContext, NugetExists {
  & "$($global:context.build_dir)\nuget.exe" @('restore', $($global:context.sln_file_info.FullName))
}

Task Build -Depends SetupContext, Clean, NugetRestore{
  Write-Host "Building $($global:context.sln_file_info.Name) ($($global:context.configuration))" -ForegroundColor Green
  $verboseLevel = "quiet"
  if ($verbose) {
    $verboseLevel = "normal"
  }
  Exec { msbuild "$($global:context.sln_file_info.FullName)" /t:ReBuild /p:Configuration=$($global:context.configuration) /v:$verboseLevel /p:OutDir=$($global:context.build_artifacts_dir) }
}


Task Test -Depends SetupContext {
  $xunit_consoleRunner = Join-Path $($global:context.sln_file_info.Directory.FullName) "\packages\xunit.runners.**\tools\xunit.console.clr4.exe"

  Get-ChildItem -Path $($global:context.build_artifacts_dir) -Filter *.Tests.dll |
  % {
    Write-Verbose "$xunit_consoleRunner @($($_.FullName), '/silent')" 
    & $xunit_consoleRunner @($_.FullName, '/silent')
    if($LastExitCode -ne 0){
      throw "failed executing tests $_.FullName. See last error."
    }
  }
}

Task PatchAssemblyInfo -Depends SetupContext {
  $version = $global:context.build_version

  Import-Module "$($global:context.build_dir)\builtmodules\Crane.PowerShell.dll"
  Update-CraneAllProjectsAssemblyInfos -SolutionContext $($global:context.solution_context) -Version $version 
  
  if ($($global:context.teamcity_build)) {
    Write-Host "##teamcity[buildNumber '$version']"
  }
}
