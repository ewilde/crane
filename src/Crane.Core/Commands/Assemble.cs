using PowerArgs;

namespace Crane.Core.Commands
{
    /// <summary>
    /// Takes an existing solution located in the supplied FolderName argument
    /// and assembles a build script based on configured of default build templates.
    /// </summary>
    /// <example>
    /// example 1
    /// <code>crane assemble SallyFx</code>
    /// </example>
    public class Assemble : ICraneCommand
    {
        [ArgRequired]
        [ArgPosition(0)]
        public string FolderName { get; set; }
    }
}
