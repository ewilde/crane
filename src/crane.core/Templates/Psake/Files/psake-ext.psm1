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

export-modulemember -function Invoke-DownloadNuget