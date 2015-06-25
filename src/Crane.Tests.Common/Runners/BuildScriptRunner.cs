using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Crane.Core;
using Crane.Core.IO;
using Crane.Core.Runners;
using log4net;

namespace Crane.Tests.Common.Runners
{
    public class BuildScriptRunner
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(BuildScriptRunner));
        private IFileManager _fileManager;

        public BuildScriptRunner()
        {
            _fileManager = new FileManager(new HostEnvironment());
        }

        public RunResult Run(string projectRootPath, string taskList = "@('PatchAssemblyInfo', 'BuildSolution', 'Test')", params string[] otherArguments)
        {
            var buildps1 = Path.Combine(_fileManager.GetShortPath(projectRootPath), "build.ps1");
            if (!File.Exists(buildps1))
            {
                throw new FileNotFoundException(string.Format("Could not find the build.ps1 in the project root directory {0}.", projectRootPath));
            }

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = projectRootPath,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = string.Format("{0}\\system32\\windowspowershell\\v1.0\\powershell.exe", Environment.GetFolderPath(Environment.SpecialFolder.Windows)),
                    Arguments = string.Format("-NoProfile -ExecutionPolicy unrestricted -Command \"{0} {1} {2}\"", 
                        buildps1, taskList,
                        otherArguments != null && otherArguments.Length > 0 ? otherArguments.Aggregate((current, next) => current + " " + next) : string.Empty)
                }
            };

            var error = new StringBuilder();
            var output = new StringBuilder();

            process.ErrorDataReceived += (sender, args) => error.Append(args.Data);
            process.OutputDataReceived += (sender, args) => output.Append(args.Data);

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();

            _log.DebugFormat("standard out: {0}  error: {1}", output, error);

            return new RunResult(
                string.Format("{0} {1}", process.StartInfo.FileName, process.StartInfo.Arguments),
                output.ToString(),
                error.ToString(),
                process.ExitCode);        
        }        
    }
}