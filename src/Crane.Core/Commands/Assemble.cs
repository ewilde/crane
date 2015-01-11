using PowerArgs;

namespace Crane.Core.Commands
{
    /// <summary>
    /// Takes an existing solution found using the FolderName argument
    /// and assembles a build script.
    /// </summary>
    /// <example>
    /// example 1
    /// <code>crane assemble SallyFx</code>
    ///
    /// This creates a new build inside the SallyFx folder.  The build scripts will be contained inside a build folder.
    ///
    ///
    /// By default the build will perform the following steps
    ///
    /// - Patch assembly info - the assembly info of your assemblies with the version found in the version.txt
    /// - Build the solution
    /// - Run your tests (if they are xunit tests at the moment only xunit is supported)
    ///
    /// Once you have the build you can invoke by running
    ///  .\build
    ///
    /// To setup in TeamCity.
    ///
    /// - Create a new build
    /// - Add one build step to invoke a powershell script and paste in the following:
    /// <code>".\build.ps1 -tasklist @("TeamCityBuildStep") -build_number %build.number% -teamcity_build"</code>
    ///
    /// That's it! You will now have a fully running CI build on TeamCity.
    ///
    /// </example>
    public class Assemble : ICraneCommand
    {
        [ArgRequired]
        [ArgPosition(0)]
        public string FolderName { get; set; }
    }
}
