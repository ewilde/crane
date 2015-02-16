using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Xbehave;

namespace Crane.Tests.Common.Context
{
    /// <summary>
    /// If you get problems running this test, it could be firewall related.
    /// Try opening the port in an admin window: netsh advfirewall firewall add rule name="Open Port 8080" dir=in action=allow protocol=TCP localport=8080
    /// </summary>
    public class NuGetServerContext
    {
        private readonly ICraneTestContext _testContext;
        private readonly Process _process;
        private readonly StringBuilder _error;
        private readonly StringBuilder _output;

        private static readonly ILog Log = LogManager.GetLogger(typeof(NuGetServerContext));
        private readonly ManualResetEvent _waitForStarted;
        private static readonly Uri Uri = new Uri("http://localhost:8080/api/");
        public const string LocalAdministratorApiKey = "fd6845f4-f83c-4ca2-8a8d-b6fc8469f746";

        public Uri ApiUri
        {
            get { return Uri; }
        }

        public string ApiKey
        {
            get { return LocalAdministratorApiKey; }
        }

        public NuGetServerContext(ICraneTestContext testContext)
        {
            Log.Info("Initializing NugetSeverContext");
            foreach (var process in Process.GetProcessesByName("Klondike.SelfHost"))
            {
                process.Kill();
                Log.InfoFormat("Klondike.SelfHost {0}", "process killed during start up");
            }

            _testContext = testContext;
            _waitForStarted = new ManualResetEvent(false);
            _process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = Path.Combine(_testContext.ToolsDirectory, "klondie", "bin"),
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = Path.Combine(_testContext.ToolsDirectory, "klondie", "bin", "Klondike.SelfHost.exe"),
                    Arguments = "--port=8080 --interactive"
                }
            };

            _error = new StringBuilder();
            _output = new StringBuilder();

            Task.Run(() =>
            {

                try
                {
                    _process.ErrorDataReceived += (sender, args) => _error.Append(args.Data);
                    _process.OutputDataReceived += (sender, args) =>
                    {
                        _output.Append(args.Data);
                        if (Output.Contains("Press <enter> to stop."))
                        {
                            _waitForStarted.Set();
                        }
                    };

                    _process.Start();
                    _process.BeginOutputReadLine();
                    _process.BeginErrorReadLine();

                    Log.Info("Nuget server started on worker thread, will wait for exit");
            
                    _process.WaitForExit();
                }
                catch (Exception exception)
                {
                    Log.Error(exception);
                }
            });

            if (!_waitForStarted.WaitOne(TimeSpan.FromSeconds(30)))
            {
                throw new Exception(string.Format("Could not start klondie{0}Standard out:{0}{1}Error out:{0}{2}",
                    System.Environment.NewLine, Output, Error));
            }
        }

        public string Output
        {
            get { return _output.ToString(); }
        }

        public string Error
        {
            get { return _error.ToString(); }
        }

        public void TearDown()
        {
            Log.Info("Tearing down nuget server");
            _process.Kill();
        }
    }
}
