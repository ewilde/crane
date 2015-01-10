#FAQ
##What are the advantages of using Crane over msbuild

Crane is a build framework that uses other build tools, such as msbuild, under the hood. However using crane you get a lot of features out of the box without the need to code them yourself.

Whilst Crane is still in it's infancy (version 0.1) below are the following features you get with crane without the need for any custom scripting or programing:

*Crane 0.1 Features*
* Creating new projects and solution with a build script already configured
* Automatic nuget download
* Nuget restore
* Compilation
* Running unit tests
* Assembly version patching
* if using git, includes commit hash and comment

Using msbuild out of the box you only get `compilation` the other features would need to be scripted using msbuild or other tools by hand

We are actively working to release new crane features in the near future. One of the big ticket items for the 0.2 release will be automatically creating `TeamCity builds`.

## Targeted Powershell & Visual Studio Versions
**Answer:** Powershell version 3.0 or greater & Visual Studio 2013

Crane builds will run with powershell. For now we are not going to target VS2012 specifically.

If you run `crane init {projectname}` and open it in VS2012 it won't compile. This is because the nuget restore behaviour changes between VS2012 and VS2013. We rely on the automatic restore behaviour in VS2013. However if you run `.\build.ps1` first in powershell it will compile as that runs the nuget restore for you.
