using System;
using System.IO;
using System.Reflection;
using Crane.Core.Utility;

namespace Crane.Core.Configuration
{
    public class CraneContext : ICraneContext
    {
        public static readonly string DefaultInstallationDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "crane");

        private DirectoryInfo _buildDirectory;
        private DirectoryInfo _sourceDirectory;
        private DirectoryInfo _craneInstallDiretory;

        public CraneContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public DirectoryInfo BuildDirectory
        {
            get
            {
                return _buildDirectory ??
                       (_buildDirectory =
                           new DirectoryInfo(Path.Combine(ProjectRootDirectory.FullName, Configuration.BuildFolderName)));
            }
        }

        public DirectoryInfo CraneInstallDirectory
        {
            get
            {
                return _craneInstallDiretory ??
                       (_craneInstallDiretory = AssemblyUtility.GetLocation(Assembly.GetExecutingAssembly()));
            }
        }

        public IConfiguration Configuration { get; private set; }

        public string ProjectName { get; set; }

        public DirectoryInfo SourceDirectory
        {
            get
            {
                return _sourceDirectory ??
                       (_sourceDirectory =
                           new DirectoryInfo(Path.Combine(ProjectRootDirectory.FullName, Configuration.SourceFolderName)));
            }
        }

        public DirectoryInfo ProjectRootDirectory { get; set; }
    }
}