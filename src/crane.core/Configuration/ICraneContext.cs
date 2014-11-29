using System.IO;

namespace Crane.Core.Configuration
{
    public interface ICraneContext
    {
        DirectoryInfo BuildDirectory { get; }
       
        IConfiguration Configuration { get; }

        DirectoryInfo CraneInstallDirectory { get; }

        DirectoryInfo ProjectRootDirectory { get; set; }
    }
}