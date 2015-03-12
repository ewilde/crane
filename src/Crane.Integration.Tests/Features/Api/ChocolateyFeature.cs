using System.Diagnostics;
using System.IO;
using Crane.Core.Api;
using Crane.Core.Configuration;
using Crane.Core.Extensions;
using Crane.Core.Runners;
using Crane.Tests.Common.Context;
using Crane.Tests.Common.FluentExtensions;
using Crane.Tests.Common.Runners;
using Xbehave;

namespace Crane.Integration.Tests.Features.Api
{
    public class ChocolateyFeature
    {
        [Scenario]
        public void can_pack_a_chocolatey_template(
            string chocolateySpecPath,
            CraneRunner craneRunner, 
            RunResult chocolateyPackResult,
            CraneTestContext craneTestContext,
            ISolutionContext solutionContext, 
            string projectDir, 
            ICraneApi craneApi,
            string buildOutputFolder,
            string chocolateyOutputFolder 
           )
        {
             "Given I have my own private copy of the crane console"
                ._(() => craneTestContext = ServiceLocator.Resolve<CraneTestContext>());

            "And I have a chocolatey specification file"
                ._(() =>
                {
                    var template = @"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd"">
  <metadata>    
    <id>chocotest</id>
    <title>crane</title>
    <iconUrl>https://cdn.rawgit.com/ewilde/crane/e220828fde0624655b4c55d89a5b70ccac9d94aa/doc/crane_128.png</iconUrl>
    <version>$version$</version>
    <authors>Edward Wilde &amp; Kevin Holditch</authors>
    <owners>Edward Wilde &amp; Kevin Holditch</owners>
    <summary>Crane: creates builds scripts so you don't have to</summary>
    <description>We hate writing build scripts and continuous integration (ci) templates every time we start a new project or when we work on a project that does not have them. I'm sure you do as well. This is where crane comes in. Invoke crane and it can build you a blank project, complete with a build or you can get crane to assemble you a build on an existing project. No more messing around for hours on a build server!</description>
    <projectUrl>https://github.com/ewilde/crane</projectUrl>
    <tags></tags>
    <copyright>2015 Edward Wilde &amp; Kevin Holditch</copyright>
    <licenseUrl>http://www.apache.org/licenses/LICENSE-2.0.html</licenseUrl>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <releaseNotes></releaseNotes>
  </metadata>
  <files>
    <file src=""$build_output$\**"" target=""tools"" />
  </files>
</package>";
                    chocolateySpecPath = Path.Combine(craneTestContext.RootDirectory, "ServiceStack.nuspec");
                    File.WriteAllText(chocolateySpecPath, template);
                });

            "And I have a run context"
                ._(() => craneRunner = new CraneRunner());

            "And I have run crane init ServiceStack"
                ._(() => craneRunner.Command(craneTestContext.BuildOutputDirectory, "crane init ServiceStack").Should().BeErrorFree());

            "And I have build the project"
                ._(() => new BuildScriptRunner().Run(Path.Combine(craneTestContext.BuildOutputDirectory, "ServiceStack"))
                    .Should().BeBuiltSuccessfulyWithAllTestsPassing().And.BeErrorFree());

            "And I have the solution context using the api"
               ._(() =>
               {
                   projectDir = Path.Combine(craneTestContext.BuildOutputDirectory, "ServiceStack");
                   craneApi = ServiceLocator.Resolve<ICraneApi>();
                   solutionContext = craneApi.GetSolutionContext(projectDir);
                   buildOutputFolder = Path.Combine(solutionContext.Path, "build-output");
                   chocolateyOutputFolder = Path.Combine(solutionContext.Path, "chocolatey-output");
               });

            "And choco is not already running"
                ._(() => Process.GetProcessesByName("choco").ForEach(process => process.Kill()));
                

            "When I use the api to create the chocolatey package"
                ._(() => chocolateyPackResult = craneApi.ChocolateyPack(solutionContext, chocolateySpecPath, buildOutputFolder, chocolateyOutputFolder, "0.0.0.1"));

            "It should have run the command without errors"
                ._(() => chocolateyPackResult.Should().BeErrorFree());

            "It should create the chocolatey package"
                ._(() => File.Exists(Path.Combine("chocolateyOutputFolder", "chocotest.0.0.0.1.nupkg")))

                .Teardown(() =>
                {
                    Process.GetProcessesByName("choco").ForEach(process => process.Kill());
                    craneTestContext.TearDown();
                });
        }
    }
}