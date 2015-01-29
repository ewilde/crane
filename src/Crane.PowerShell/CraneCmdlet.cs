using System;
using System.Management.Automation;
using Crane.Core.Configuration;
using Crane.Core.Extensions;
using log4net;

namespace Crane.PowerShell
{
    public abstract class CraneCmdlet : PSCmdlet
    {
        protected CraneCmdlet()
        {
            ServiceLocator.BuildUp(this);
        }

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