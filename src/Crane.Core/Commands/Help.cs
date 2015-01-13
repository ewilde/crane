using PowerArgs;

namespace Crane.Core.Commands
{
    /// <summary>
    /// Get help for a crane command
    /// </summary>
    public class Help : ICraneCommand
    {
        [ArgRequired]
        [ArgPosition(0)]
        public string Command { get; set; }
    }
}