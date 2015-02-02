using System.Management.Automation;
using Crane.Core.Api;
using Crane.Core.Api.Model;
using log4net;

namespace Crane.PowerShell
{
    [Cmdlet("Update", "CraneAssemblyInfo"), OutputType(typeof(ISolutionContext))]
    public class UpdateCraneAssemblyInfo : CraneCmdlet
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(UpdateCraneAssemblyInfo));

        [Parameter(Position = 0, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, Mandatory = true)]
        public Project Project
        {
            get;
            set;
        }

        public ICraneApi Api { get; set; }

        protected override ILog Log
        {
            get { return _log; }
        }

        internal override void Process()
        {
            Api.PatchAssemblyInfo(Project.AssemblyInfo);
            WriteVerbose(string.Format("AssemblyInfo file: {0} Updated.", Project.AssemblyInfo.Path));
        }
    }
}