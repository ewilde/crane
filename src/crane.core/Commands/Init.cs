using PowerArgs;

namespace Crane.Core.Commands
{
    public class Init : ICraneCommand
    {

        [ArgRequired]
        [ArgPosition(0)]
        public string ProjectName { get; set; }
    }
}
