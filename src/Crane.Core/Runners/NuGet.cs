using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Crane.Core.IO;

namespace Crane.Core.Runners
{
    public class NuGet : INuGet
    {
        private readonly IFileManager _fileManager;

        public NuGet(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }

        public RunResult Publish(string nugetExePath, string nuGetPackagePath, string source, string apiKey)
        {
            var result = GeneralProcessRunner.Run(nugetExePath, string.Format("push \"{0}\" -Source {1} -ApiKey {2}", nuGetPackagePath, source, apiKey));
            return result;
        }

        public RunResult Pack(string nugetExePath, string nuGetSpecPath, string outputDirectory, IEnumerable<Tuple<string, string>> properties)
        {
            _fileManager.EnsureDirectoryExists(new DirectoryInfo(outputDirectory));
            var propertyArgs = new StringBuilder();
            foreach (var property in properties)
            {
                propertyArgs.AppendFormat("{0}={1};", property.Item1, property.Item2);
            }

            var result = GeneralProcessRunner.Run(nugetExePath, string.Format("pack \"{0}\" -OutputDirectory \"{1}\" -Properties \"{2}\"",
                nuGetSpecPath, outputDirectory, propertyArgs));

            return result;
        }

        public bool ValidateResult(RunResult result)
        {
            return result.ExitCode == 0 &&
                   (result.StandardOutput == null || !result.StandardOutput.Contains("invalid arguments")) &&
                   string.IsNullOrEmpty(result.ErrorOutput != null ? result.ErrorOutput.Trim() : null);
        }
    }
}