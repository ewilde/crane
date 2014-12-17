function Invoke-DownloadNuget()
{
  param([string] $destination)
    $scriptPath = Split-Path $MyInvocation.InvocationName
    $nugetFile = Join-Path $destination NuGet.exe 
    
    if (Test-Path $nugetFile)
    {
        return
    }

    ((new-object net.webclient).DownloadFile('http://www.nuget.org/nuget.exe', $nugetFile))
}

function Get-Git-Commit
{
	$gitLog = git log --oneline -1
	
	if ($LASTEXITCODE -gt 0)
	{
		return ""
	}
	
	return $gitLog.Split(' ')[0]
}
function Get-Version-From-Git-Tag
{
  $gitTag = git describe --tags --abbrev=0
  return $gitTag.Replace("v", "") + ".0"
}
function Invoke-GenerateAssemblyInfo
{
param(
	[string]$clsCompliant = "true",
	[string]$title, 
	[string]$description, 
	[string]$company, 
	[string]$product, 
	[string]$copyright, 
	[string]$version,
	[string]$file = $(throw "file is a required parameter.")
)
  $commit = Get-Git-Commit
  $asmInfo = "using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
[assembly: CLSCompliantAttribute($clsCompliant )]
[assembly: ComVisibleAttribute(false)]
[assembly: AssemblyTitleAttribute(""$title"")]
[assembly: AssemblyDescriptionAttribute(""$description"")]
[assembly: AssemblyCompanyAttribute(""$company"")]
[assembly: AssemblyProductAttribute(""$product"")]
[assembly: AssemblyCopyrightAttribute(""$copyright"")]
[assembly: AssemblyVersionAttribute(""$version"")]
[assembly: AssemblyInformationalVersionAttribute(""$version / $commit"")]
[assembly: AssemblyFileVersionAttribute(""$version"")]
[assembly: AssemblyDelaySignAttribute(false)]
"
	$dir = [System.IO.Path]::GetDirectoryName($file)
    if ([System.IO.Directory]::Exists($dir) -eq $false)
	{
		Write-Host "Creating directory $dir"
		[System.IO.Directory]::CreateDirectory($dir)
	}
	Write-Host "Generating assembly info file: $file"
	Write-Output $asmInfo > $file
}
export-modulemember -function Invoke-DownloadNuget, Invoke-GenerateAssemblyInfo