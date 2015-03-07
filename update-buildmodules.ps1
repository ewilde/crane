@('.\build\builtmodules';'.\src\Crane.Templates\build\Psake\build\builtmodules') | % {
    copy -Path .\build-output\Autofac.dll -Destination $_ -Force
    copy -Path .\build-output\Crane.Core.dll -Destination $_ -Force
    copy -Path .\build-output\Crane.PowerShell.dll -Destination $_ -Force
    copy -Path .\build-output\FubuCore.dll -Destination $_ -Force
    copy -Path .\build-output\FubuCsProjFile.dll -Destination $_ -Force
    copy -Path .\build-output\log4net.dll -Destination $_ -Force
    copy -Path .\build-output\PowerArgs.dll -Destination $_ -Force
}