using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using log4net;
using Xunit;

namespace Crane.Tests.Common.Context
{
    /// <summary>
    /// If you get problems running this test, it could be firewall related.
    /// Try opening the port in an admin window: netsh advfirewall firewall add rule name="Open Port 8888" dir=in action=allow protocol=TCP localport=8888
    /// </summary>
    public class NuGetServerContext
    {
        private ICraneTestContext _testContext;
        private Process _process;
        private StringBuilder _error;
        private StringBuilder _output;

        private static readonly ILog Log = LogManager.GetLogger(typeof(NuGetServerContext));
        private ManualResetEvent _waitForStarted;
        private static readonly Uri BaseUri = new Uri("http://localhost:8888");
        public const string LocalAdministratorApiKey = "fd6845f4-f83c-4ca2-8a8d-b6fc8469f746";

        public Uri ApiUri
        {
            get { return new Uri(BaseUri, "api"); }
        }

        public string ApiKey
        {
            get { return LocalAdministratorApiKey; }
        }

        public NuGetServerContext(ICraneTestContext testContext)
        {
            Log.Debug("Initializing NugetSeverContext");
            Initialize(testContext);
        }

        private void Initialize(ICraneTestContext testContext)
        {
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
                    Arguments = "--port=8888 --interactive"
                }
            };

            _error = new StringBuilder();
            _output = new StringBuilder();

            Task.Run(() =>
            {
                try
                {
                    _process.ErrorDataReceived += (sender, args) => _error.AppendLine(args.Data);
                    _process.OutputDataReceived += (sender, args) =>
                    {
                        _output.AppendLine(args.Data);
                        if (Output.Contains("Press <enter> to stop."))
                        {
                            _waitForStarted.Set();
                        }
                    };

                    Log.DebugFormat("Starting {0} {1}", _process.StartInfo.FileName, _process.StartInfo.Arguments);
                    _process.Start();
                    _process.BeginOutputReadLine();
                    _process.BeginErrorReadLine();

                    Log.Debug("Nuget server started on worker thread, will wait for exit");

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
            Log.Debug("Tearing down nuget server");
            try
            {
                _process.Kill();
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        public bool PackageExists(string name, string version)
        {
            HttpResponseMessage result = null;
            dynamic response;
            try
            {
                var client = new HttpClient { BaseAddress = BaseUri };
                result = client.GetAsync(string.Format("api/packages/{0}/{1}", name, version)).Result;
                response = result.Content.ReadAsAsync<dynamic>().Result;

                result.IsSuccessStatusCode.Should().BeTrue();
                response.name.Value.Should().Equals(name);
                response.version.Value.Should().Equals(version);
                return true;
            }
            catch (Exception exception)
            {
                Log.Error(exception);
                return false;
            }
        }
    }
}
