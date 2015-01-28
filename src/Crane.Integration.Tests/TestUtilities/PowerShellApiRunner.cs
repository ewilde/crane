using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Timers;
using log4net;

namespace Crane.Integration.Tests.TestUtilities
{
    public class PowerShellApiRunner
    {
        private readonly ICraneTestContext _testContext;
        private static readonly ILog _log = LogManager.GetLogger(typeof(PowerShellApiRunner));
        private readonly System.Timers.Timer _timer;
        private bool _running;
        private Process _process;


        public PowerShellApiRunner(ICraneTestContext testContext, TimeSpan timeout)
        {
            _testContext = testContext;
            _timer = new Timer
            {
                Interval = timeout.TotalMilliseconds
            };
             
            _timer.Elapsed += OnTimerElapsed;
        }

        public TimeSpan Timeout { get; set; }

        public RunResult Run(string apiCommand)
        {
            _process = new Process
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

            _process.ErrorDataReceived += (sender, args) => error.Append(args.Data);
            _process.OutputDataReceived += (sender, args) => output.Append(args.Data);

            _process.Start();
            _running = true;
            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();

            _process.WaitForExit();
            _running = false;
            _log.DebugFormat("standard out: {0}  error: {1}", output, error);
            return new RunResult
            {
                StandardOutput = output.ToString(),
                ErrorOutput = error.ToString(),
                ExitCode = _process.ExitCode
            };
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            if (_running)
            {
                _running = false;
                _process.Close();            
            }
        }
    }
}