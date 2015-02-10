# Features
1. [Build options](#1options)
2. [Compile](#2compile)
3. [Test](#3Test)
4. [Package and publish using nuget](#4nuget)
5. [Package and publish using chocolatey](#5choco)
6. [Build targets](#6targets)
# 1. Options

```bash

build.ps1
		-configuration		| default is "Debug".
		-build_number		| default is 0, used as the build number in assembly version information.
		-teamcity_build		| switch defaults to false. If supplied outputs version information to.teamcity during the build, so teamcity build version matches build artifact versions.
		-chocolatey_api_key	| api key used to publish chocolatey packages. see: ChocolateyPublishPackage targets
		-chocolatey_api_url	| default is https://chocolatey.org/url. The url of chocolatey server to publish packages to. 
		-nuget_api_key		| api key used to publish nuget packages. see: NugetPublishPackage targets
		-nuget_api_url		| default is https://nuget.org/api/v2/. The url of nuget server to publish packages to. 
		-verbose			| switch defaults to false. If supplied prints out detailed logging to the console.
```

# 2. Compile
# 3. Test
# 4. Nuget
If you supply a nuget api key to [build]
# 5. Choco
# 6. Build targets
## Default
## ChocolateyPublishPackage
## NugetPublish
**Required build.ps1 parameters**: `nuget_api_key`. 


**Description**

Call this target in your build to package and publish to nuget. Assumes that your nuget spec file is the same as your project file. I.e. `Sally.fx.csproj` has `Sally.fx.nuspec` in the same folder as the project file.
