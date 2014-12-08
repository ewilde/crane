using PowerArgs;

namespace Crane.Core.Commands
{
    public class InitCommand
    {

        [ArgRequired]
        [ArgPosition(0)]
        public string ProjectName { get; set; }
    }
}
