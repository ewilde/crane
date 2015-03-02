using System.Diagnostics;
using System.Text;

namespace Crane.Tests.Common.Runners
{
    /// <summary>
    /// Runs a process, capturing the standard output, error output and exit code.
    /// </summary>
    public static class GeneralProcessRunner
    {
        public static ProcessResult Run(string fileName, string arguments, string workingDirectory = null)
        {
            var error = new StringBuilder();
            var output = new StringBuilder();

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = fileName,
                    Arguments = arguments,
                }
            };

            if (workingDirectory != null)
            {
                process.StartInfo.WorkingDirectory = workingDirectory;
            }

            process.ErrorDataReceived += (sender, args) => error.Append(args.Data);
            process.OutputDataReceived += (sender, args) => output.Append(args.Data);

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();

            return new ProcessResult
            {
                StandardOutput = output.ToString(),
                ErrorOutput = error.ToString(),
                ExitCode = process.ExitCode
            };
        }

    }

    public class ProcessResult
    {
        public string StandardOutput { get; set; }
        public string ErrorOutput { get; set; }
        public int ExitCode { get; set; }
    }
}