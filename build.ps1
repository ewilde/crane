param(
    [Parameter(Position=0,Mandatory=0)]
    [string[]]$tasklist = @('Default'),
    [Parameter(Position=1,Mandatory=0)]
    [string]$configuration = 'Debug',
    [int]$build_number = 0,
	[switch]$teamcity_build
)

Write-Host $teamcityBuild
$scriptPath = Split-Path $MyInvocation.InvocationName
Import-Module (join-path $scriptPath 'build\psake.psm1')
Invoke-Psake -framework '4.0' -buildFile (join-path $scriptPath 'build\default.ps1') -taskList $tasklist -properties @{
	"configuration"="$configuration";
	"build_number"="$build_number";
	"teamcityBuild"="$($teamcity_build.ToBool())"}