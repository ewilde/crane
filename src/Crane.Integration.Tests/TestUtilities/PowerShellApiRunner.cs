using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using log4net;

namespace Crane.Integration.Tests.TestUtilities
{
    public class PowerShellApiRunner
    {
        private readonly ICraneTestContext _testContext;
        private static readonly ILog _log = LogManager.GetLogger(typeof(PowerShellApiRunner));

        public PowerShellApiRunner(ICraneTestContext testContext)
        {
            _testContext = testContext;
        }

        public RunResult Run(string apiCommand)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = _testContext.BuildOutputDirectory,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = string.Format("{0}\\system32\\windowspowershell\\v1.0\\powershell.exe", Environment.GetFolderPath(Environment.SpecialFolder.Windows)),
                    Arguments = string.Format("-NoProfile -ExecutionPolicy unrestricted -Command \"Import-Module {0};{1} ", Path.Combine(_testContext.BuildOutputDirectory, "Crane.Core.dll"), apiCommand)
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

            return new RunResult
            {
                StandardOutput = output.ToString(),
                ErrorOutput = error.ToString(),
                ExitCode = process.ExitCode
            };
        }
    }
}