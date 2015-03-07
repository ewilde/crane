using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Crane.Core.Runners;
using log4net;

namespace Crane.Tests.Common.Runners
{
    public class CraneRunner
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(CraneRunner));

        public RunResult Command(string path, string command)
        {
            _log.DebugFormat("Running process {0} using path {1}", command, path);
            var error = new StringBuilder();
            var output = new StringBuilder();

            var arguments = command.Split(' ').ToList();
            arguments.RemoveAt(0);

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = path,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = Path.Combine(path, @"crane.exe"),
                    Arguments = string.Join(" ", arguments)
                }
            };


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