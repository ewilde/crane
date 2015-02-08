using System.Management.Automation;
using Crane.Core.Api;
using Crane.Core.Api.Model;
using log4net;

namespace Crane.PowerShell
{
    [Cmdlet("Update", "CraneAllProjectsAssemblyInfos")]
    public class UpdateCraneAllProjectsAssemblyInfos : CraneCmdlet
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(UpdateCraneAllProjectsAssemblyInfos));

        [Parameter(Position = 0, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, Mandatory = true)]
        public ISolutionContext SolutionContext
        {
            get;
            set;
        }

        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, Mandatory = true)]
        public string Version
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
            Api.PatchSolutionAssemblyInfo(SolutionContext, Version);
        }
    }
}