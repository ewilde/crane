using PowerArgs;

namespace Crane.Core.Commands
{
    public class Help : ICraneCommand
    {
        [ArgRequired]
        [ArgPosition(0)]
        public string Command { get; set; }
    }
}