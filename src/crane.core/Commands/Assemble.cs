using PowerArgs;

namespace Crane.Core.Commands
{
    public class Assemble : ICraneCommand
    {
        [ArgRequired]
        [ArgPosition(0)]
        public string FolderName { get; set; }
    }
}
