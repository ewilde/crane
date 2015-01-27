Task NugetExists {

  $nugetFile = Join-Path "$($global:context.build_dir)" NuGet.exe

  if (Test-Path $nugetFile){
    return
  }

  ((new-object net.webclient).DownloadFile('http://www.nuget.org/nuget.exe', $nugetFile))
}

Task Clean -Depends SetupContext {
  Write-Host "Creating build-output directory" -ForegroundColor Green
  if (Test-Path $($global:context.build_artifacts_dir)){
    rd $($global:context.build_artifacts_dir) -rec -force | out-null
  }

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
    Debug("$xunit_consoleRunner @($($_.FullName), '/silent')")
    & $xunit_consoleRunner @($_.FullName, '/silent')
    if($LastExitCode -ne 0){
      throw "failed executing tests $_.FullName. See last error."
    }
  }
}

function Debug($message){
  Write-Verbose $message
}


function Get-GitCommit{
  $gitLog = git log --oneline -1
  return $gitLog.Split(' ')[0]
}

function Get-VersionFromGitTag{
  $gitTag = git describe --tags --abbrev=0
  return $gitTag.Replace("v", "") + ".0"
}

function Read-GitCommitMessage{
  $gitLog = git log --oneline -1
  
  if ($LASTEXITCODE -gt 0)
  {
    return ""
  }
  
  return $gitLog
}

function Invoke-GenerateAssemblyInfo{
  param(
    [string]$title,
    [string]$description,
    [string]$company,
    [string]$product,
    [string]$copyright,
    [string]$version,
    [string]$file = $(throw "file is a required parameter.")
  )

  if ($($global:context.is_git_repo)){
    $commit = Read-GitCommitMessage
    $versionInfo = "$version / $commit"
  }else{
    $versionInfo = "$version"
  }

  $asmInfo = "using System;
  using System.Reflection;
  using System.Runtime.CompilerServices;
  using System.Runtime.InteropServices;

  [assembly: CLSCompliantAttribute(true)]
  [assembly: ComVisibleAttribute(false)]
  [assembly: AssemblyTitleAttribute(""$title"")]
  [assembly: AssemblyDescriptionAttribute(""$description"")]
  [assembly: AssemblyCompanyAttribute(""$company"")]
  [assembly: AssemblyProductAttribute(""$product"")]
  [assembly: AssemblyCopyrightAttribute(""$copyright"")]
  [assembly: AssemblyVersionAttribute(""$version"")]
  [assembly: AssemblyInformationalVersionAttribute(""$versionInfo"")]
  [assembly: AssemblyFileVersionAttribute(""$version"")]
  [assembly: AssemblyDelaySignAttribute(false)]
  "

  $dir = [System.IO.Path]::GetDirectoryName($file)
  if ([System.IO.Directory]::Exists($dir) -eq $false){
    Write-Host "Creating directory $dir"
    [System.IO.Directory]::CreateDirectory($dir)
  }
  Write-Host "Generating assembly info file: $file"
  Write-Output $asmInfo > $file
}

Task PatchAssemblyInfo -Depends SetupContext {
  $version = $global:context.build_version
  $assemblyInfoFiles = Get-ChildItem -Path $($global:context.root_dir) -Filter "AssemblyInfo.cs" -Recurse  | 
                        Where { -not $_.FullName.Contains("Templates\") -and -not $_.FullName.Contains("\bin\") }
                        
  ForEach ($assemblyInfoFile in $assemblyInfoFiles){
      $assemblyTitle = $assemblyInfoFile.Directory.Parent.Name
      $assemblyDescription = $assemblyTitle.Replace(".", " ") + " functionality"
      
      Invoke-GenerateAssemblyInfo  -title $assemblyTitle -description $assemblyDescription -version $version -file $assemblyInfoFile.FullName


  }
  
  if ($($global:context.teamcity_build)) {
    Write-Host "##teamcity[buildNumber '$version']"
  }
}
