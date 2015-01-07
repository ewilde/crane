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
