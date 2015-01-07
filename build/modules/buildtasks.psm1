
Task Clean {
  Write-Host "Creating build-output directory" -ForegroundColor Green
  if (Test-Path $($global:context.build_artifacts_dir)){
    rd $($global:context.build_artifacts_dir) -rec -force | out-null
  }

  mkdir $($global:context.build_artifacts_dir) | out-null

  Write-Host "Cleaning $($global:context.sln_file_info.Name) ($($global:context.configuration))" -ForegroundColor Green
  Exec { msbuild $($global:context.sln_file_info.FullName) /t:Clean /p:Configuration=$($global:context.configuration) /v:quiet }
}

Task NugetRestore -Depends NugetExists {
  & "$($global:context.build_dir)\nuget.exe" @('restore', $($global:context.sln_file_info.FullName))
}

Task Build -Depends Clean, NugetRestore {
  Write-Host "Building $($global:context.sln_file_info.Name) ($($global:context.configuration))" -ForegroundColor Green
  $verboseLevel = "quiet"
  if ($verbose) {
    $verboseLevel = "normal"
  }
  Exec { msbuild "$($global:context.sln_file_info.FullName)" /t:ReBuild /p:Configuration=$($global:context.configuration) /v:$verboseLevel /p:OutDir=$($global:context.build_artifacts_dir) }
}
