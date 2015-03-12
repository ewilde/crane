Task ChocolateyExists{
  try{
    & choco
  }catch{
    iex ((new-object net.webclient).DownloadString('https://chocolatey.org/install.ps1'))
  }
}

Task ChocolateyBuildPackage -Depends ChocolateyExists{
  $choco_output_dir = "$($global:context.root_dir)\chocolatey-output"
  $choco_nuspec = "$($global:context.root_dir)\src\Crane.Chocolatey\crane.nuspec"

  Import-Module "$($global:context.build_dir)\builtmodules\Crane.PowerShell.dll"
  Invoke-CraneChocolateyPack $global:context.solution_context -ChocolateySpecPath -BuildOutputPath $global:context.build_artifacts_dir -ChocolateyOutputPath $choco_output_dir -Version $global:context.build_version | % {
    $_.StandardOutput
  }  
}

Task ChocolateyPublishPackage -Depends ChocolateyBuildPackage{
  Get-ChildItem "$($global:context.root_dir)\chocolatey-output" -Filter *.nupkg |
  Foreach-Object{
    & "$($global:context.build_dir)\nuget.exe" @('push', $_.FullName, "-s", $($global:context.chocolatey_api_url), $($global:context.chocolatey_api_key))
  }
}
