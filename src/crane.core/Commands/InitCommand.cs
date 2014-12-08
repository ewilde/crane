using PowerArgs;

namespace Crane.Core.Commands
{
    public class InitCommand
    {
        public string Name { get { return "Init"; } }

        [ArgRequired]
        [ArgPosition(0)]
        public string ProjectName { get; set; }
    }
}
