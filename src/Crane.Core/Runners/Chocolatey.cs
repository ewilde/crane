using System;
using System.IO;
using System.Text;
using Crane.Core.IO;

namespace Crane.Core.Runners
{
    public class Chocolatey : IChocolatey
    {
        private readonly IFileManager _fileManager;

        public Chocolatey(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }

        public RunResult Pack(string chocolateyExePath, string chocolateySpecPath, string chocolateyOutputPath,
            string buildArtifactsOutputPath, string version)
        {
            var sourceSpec = new FileInfo(chocolateySpecPath);
            var targetSpec = Path.Combine(chocolateyOutputPath, sourceSpec.Name);

            _fileManager.EnsureDirectoryExists(new DirectoryInfo(chocolateyOutputPath));
            _fileManager.WriteAllText(
                targetSpec, 
                _fileManager.ReadAllText(chocolateySpecPath)
                    .Replace("$build_output$", buildArtifactsOutputPath)); // note even though cpack uses nuget it doesn't pass in -properties so we have to do this replacement manually
            
            var result = GeneralProcessRunner.Run(chocolateyExePath, string.Format("pack {0} --version {1} --confirm",
                targetSpec,
                version), null, TimeSpan.FromSeconds(10));

            return result;
        }
    }
}