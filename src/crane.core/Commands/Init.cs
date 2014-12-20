using PowerArgs;

namespace Crane.Core.Commands
{
    /// <summary>
    /// Initializes a new project
    /// </summary>
    /// <example>
    /// EXAMPLE 1
    /// <code>
    ///     crane init SallyFx
    /// </code>
    /// This example initializes a new project 'SallyFx' in the current directory
    /// </example>
    public class Init : ICraneCommand
    {

        [ArgRequired]
        [ArgPosition(0)]
        public string ProjectName { get; set; }
    }
}
