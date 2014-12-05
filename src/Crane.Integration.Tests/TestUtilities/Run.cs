using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Crane.Integration.Tests.TestUtilities
{
    public class Run
    {
        public RunResult Command(string path,string command)
        {
            var error = new StringBuilder();
            var output = new StringBuilder();

            var arguments = command.Split(' ').ToList();
            arguments.RemoveAt(0);

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
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

            return new RunResult
            {
                StandardOutput = output.ToString(),
                ErrorOutput = error.ToString(),
                ExitCode = process.ExitCode
            };
        }

        

    }
}