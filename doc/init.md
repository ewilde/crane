`usage: crane init <project name>`

Initializes a new project in the current working directory using the
default crane project and build templates.

**example 1**  
`crane init SallyFx`

This example initializes a new project 'SallyFx'. This will create the
following new directory structure:
```
SallyFx
|   build.ps1
|
+---build
|       default.ps1
|       NuGet.exe
|       psake-ext.psm1
|       psake.ps1
|       psake.psm1
|
\---src
    |   SallyFx.sln
    |
    +---.nuget
    |       packages.config
    |
    +---SallyFx
    |   |   Calculator.cs
    |   |   SallyFx.csproj
    |   |
    |   \---Properties
    |           AssemblyInfo.cs
    |
    \---SallyFx.UnitTests
        |   CalculatorFeature.cs
        |   packages.config
        |   SallyFx.UnitTests.csproj
        |
        \---Properties
                AssemblyInfo.cs
```

