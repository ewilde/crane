Task GenerateDocs -Depends SetupContext {
  $crane_exe = "$($context.build_artifacts_dir)\crane.exe"
  Write-Host $crane_exe @("gendoc")
  & $crane_exe @("gendoc")
}
