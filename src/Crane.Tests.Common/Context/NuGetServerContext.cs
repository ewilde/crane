using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Security.Principal;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Crane.Core.Extensions;
using Crane.Core.Runners;
using Crane.Tests.Common.Runners;
using FluentAssertions;
using log4net;
using Xunit;

namespace Crane.Tests.Common.Context
{
    /// <summary>
    /// If you get problems running this test, it could be firewall related.
    /// Try opening the port in an admin window: netsh advfirewall firewall add rule name="Open Port 52545" dir=in action=allow protocol=TCP localport=52545
    /// </summary>
    
    public class NuGetServerContext
    {
        private ICraneTestContext _testContext;

        private static readonly ILog Log = LogManager.GetLogger(typeof(NuGetServerContext));
        private const int PortNumber = 52545;
        private const string ServiceName = "Klondike";
        private static readonly Uri BaseUri = new Uri(string.Format("http://{0}:{1}", System.Environment.MachineName, PortNumber));
        private static readonly TimeSpan WaitForServiceStatusTimeout = TimeSpan.FromSeconds(10);
        public const string LocalAdministratorApiKey = "fd6845f4-f83c-4ca2-8a8d-b6fc8469f746";
        private const int ServiceDoesNotExist = 1060;

        public Uri Source
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
            _testContext = testContext;
           
            KillAllKlondikeProcesses();

            var binPath = Path.Combine(_testContext.ToolsDirectory, "klondie", "bin", "Klondike.SelfHost.exe");
            var arguments = string.Format("--port={0}", PortNumber);
            
            DeleteService();
            CreateService(binPath, arguments);
            StartService();

            Log.Debug("nuGet server started");
        }

        private void StartService()
        {
            Log.DebugFormat(@"Starting service {0}", ServiceName);
            var controller = new ServiceController(ServiceName);
            controller.Start();
            controller.WaitForStatus(ServiceControllerStatus.Running, WaitForServiceStatusTimeout);
            Log.DebugFormat(@"Started service {0}", ServiceName);
        }

        private void StopService()
        {
            Log.DebugFormat(@"Stopping service {0}", ServiceName);
            var controller = new ServiceController(ServiceName);
            controller.Stop();
            controller.WaitForStatus(ServiceControllerStatus.Stopped, WaitForServiceStatusTimeout);
            Log.DebugFormat(@"Stopped service {0}", ServiceName);
        }

        private void CreateService(string binPath, string arguments)
        {
            WindowsIdentity.GetCurrent().IsElevated().Should().BeTrue("process was not running as admin. You cannot create a service without running as administrator.");
            var scArgs = string.Format("create {0} start=auto binpath=\"{1} {2}\"", ServiceName, binPath, arguments);
            var result = GeneralProcessRunner.Run(@"C:\Windows\system32\sc.exe", scArgs);

            if (result.ExitCode != 0)
            {
                throw new Exception(string.Format(@"Could not create service: C:\Windows\system32\sc.exe {0}{1}Standard output: {2}{1}Error output:{3}", 
                    scArgs, Environment.NewLine, result.StandardOutput, result.ErrorOutput));
            }

            Log.DebugFormat(@"Creating service C:\Windows\system32\sc.exe {0}", scArgs);
            Log.DebugFormat(@"Creating service standard output: {0}", result.StandardOutput);
            Log.DebugFormat(@"Creating service error output: {0}", result.ErrorOutput);
        }

        private void DeleteService()
        {
            var scArgs = string.Format("delete {0}", ServiceName);
            var result = GeneralProcessRunner.Run(@"C:\Windows\system32\sc.exe", scArgs);

            if (result.ExitCode != 0 && result.ExitCode != ServiceDoesNotExist)
            {
                throw new Exception(string.Format(@"Could not delete service: C:\Windows\system32\sc.exe {0}{1}Standard output: {2}{1}Error output:{3}{1}Exit code:{4}",
                    scArgs, Environment.NewLine, result.StandardOutput, result.ErrorOutput, result.ExitCode));
            }

            Log.DebugFormat(@"Deleting service C:\Windows\system32\sc.exe {0}", scArgs);
            Log.DebugFormat(@"Deleting service standard output: {0}", result.StandardOutput);
            Log.DebugFormat(@"Deleting service error output: {0}", result.ErrorOutput);
            Log.DebugFormat(@"Deleting service exit code: {0}", result.ErrorOutput);
        }

        private static void KillAllKlondikeProcesses()
        {
            foreach (var process in Process.GetProcessesByName("Klondike.SelfHost"))
            {
                process.Kill();
                Log.InfoFormat("Klondike.SelfHost {0}", "process killed during start up");
            }
        }

        public string Output
        {
            get
            {
                return
                    File.ReadAllText(Path.Combine(_testContext.ToolsDirectory, "klondie", "bin", "Logs", "Klondike.log"));
            }
        }

        public int PackageCount
        {
            get
            {
                try
                {
                    var client = new HttpClient { BaseAddress = BaseUri };
                    var result = client.GetAsync("api/packages").Result;
                    var response = result.Content.ReadAsAsync<dynamic>().Result;

                    Log.Debug(response.ToString());
                    return (int)response.count.Value;
                }
                catch (Exception exception)
                {
                    Log.Error(exception.ToString());
                    return -1;
                }
            }
        }

        public void TearDown()
        {
            Log.Debug("Tearing down nuGet server");
            try
            {
                StopService();
                DeleteService();
                KillAllKlondikeProcesses();
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        public bool PackageExists(string name, string version)
        {
            try
            {
                var client = new HttpClient { BaseAddress = BaseUri };
                var result = client.GetAsync(string.Format("api/packages/{0}/{1}", name, version)).Result;
                var response = result.Content.ReadAsAsync<dynamic>().Result;

                result.IsSuccessStatusCode.Should()
                    .BeTrue("result should be a 200 code.{0}Request message{1}{0}Result message: {2}.", Environment.NewLine,
                        result.RequestMessage, result.ToString());
                ((string)response.id.Value).Should().Be(name);
                ((string)response.version.Value).Should().Be(version);
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
