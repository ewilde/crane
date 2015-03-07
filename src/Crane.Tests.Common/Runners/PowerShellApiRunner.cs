using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using Crane.Core.Runners;
using Crane.Tests.Common.Context;
using log4net;

namespace Crane.Tests.Common.Runners
{
    public class PowerShellApiRunner
    {
        private readonly ICraneTestContext _testContext;
        private static readonly ILog _log = LogManager.GetLogger(typeof(PowerShellApiRunner));
        private readonly System.Timers.Timer _timer;
        private bool _running;
        private Process _process;
        private StringBuilder _output;
        private StringBuilder _error;
        private bool _timerHasElasped;


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
        public RunResult Run(string apiCommand, params Object[] commandArgs)
        {
            apiCommand = string.Format(apiCommand, commandArgs);
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
                    Arguments = string.Format("-NoProfile -ExecutionPolicy unrestricted -Command \"Import-Module {0};{1}\"", Path.Combine(_testContext.BuildOutputDirectory, "Crane.PowerShell.dll"), apiCommand)
                }
            };


            _error = new StringBuilder();
            _output = new StringBuilder();

            _process.ErrorDataReceived += (sender, args) => _error.Append(args.Data);
            _process.OutputDataReceived += (sender, args) => _output.Append(args.Data);
			_log.DebugFormat("About to execture powershell: {0} {1}", _process.StartInfo.FileName, _process.StartInfo.Arguments);

            _timer.Start();

            _running = true;
            _process.Start();
            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();

            _process.WaitForExit();
            _running = false;

            if (_timerHasElasped)
            {
                throw new Exception(string.Format("Timeout running PowerShell command {0}{1}{2}{3}", _process.StartInfo.FileName, _process.StartInfo.Arguments, Environment.NewLine, CreateRunResult()));
            }
            _log.DebugFormat("standard out: {0}  error: {1}", _output, _error);
            return CreateRunResult();
        }

        private RunResult CreateRunResult()
        {
            return new RunResult(
                string.Format("{0} {1}", _process.StartInfo.FileName, _process.StartInfo.Arguments),
                _output.ToString(),
                _error.ToString(),
                GetExitCode());
        }

        private int GetExitCode()
        {
            var result = -1;
            try
            {
                result = _process.ExitCode;
            }
            catch (InvalidOperationException)
            {
            }

            return result;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _timerHasElasped = true;
            _timer.Stop();
            if (_running)
            {
                _running = false;
                TerminateProcess(_process.Handle, (uint) 101);
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);
    }
}