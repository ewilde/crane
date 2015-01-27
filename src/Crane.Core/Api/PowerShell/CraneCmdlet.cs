using System;
using System.Management.Automation;
using log4net;
using Crane.Core.Extensions;
namespace Crane.Core.Api.PowerShell
{
    internal abstract class CraneCmdlet : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            try
            {
                this.Initialize();
                Process();
            }
            catch (Exception exception)
            {
                this.Log.ErrorFormat("Error processing command. Error: {0}", exception.ToString());
                throw;
            }
        }

        protected abstract ILog Log { get; }

        protected virtual void Initialize()
        {
        }

        internal abstract void Process();

        internal static bool RunningInPowerShell
        {
            get { return System.Diagnostics.Process.GetCurrentProcess()
                    .ProcessName.Contains("powershell", StringComparison.InvariantCultureIgnoreCase); }
        }
    }
}