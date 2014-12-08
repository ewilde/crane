using PowerArgs;

namespace Crane.Core.Commands.CommandArguments
{
    public class InitCommandArgs
    {
        [ArgPosition(0)]
        [ArgRequired]
        public string ProjectName { get; set; }
    }
}
