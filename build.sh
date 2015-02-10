mono /usr/bin/nuget restore src/Crane.sln
xbuild src/Crane.sln /verbosity:quiet /target:rebuild /p:configuration=Debug-Mono
mono src/packages/xunit.runners.1.9.2/tools/xunit.console.clr4.exe src/Crane.Core.Tests/bin/Debug/Crane.Core.Tests.dll
mono src/packages/xunit.runners.1.9.2/tools/xunit.console.clr4.exe src/Crane.Integration.Tests/bin/Debug/Crane.Integration.Tests.dll
