$scriptPath = Split-Path $MyInvocation.InvocationName
Import-Module (join-path $scriptPath 'build\psake.psm1')
Invoke-Psake -framework '4.0' -buildFile (join-path $scriptPath 'build\default.ps1')