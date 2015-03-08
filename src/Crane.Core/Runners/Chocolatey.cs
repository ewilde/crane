using System.IO;
using System.Text;
using Crane.Core.IO;

namespace Crane.Core.Runners
{
    public class Chocolatey : IChocolatey
    {
        public RunResult Pack(string chocolateyExePath, string chocolateySpecPath)
        {
            var result = GeneralProcessRunner.Run(chocolateyExePath, string.Format("pack {0}",
                chocolateyExePath));

            return result;
        }
    }
}