using System.Management.Automation;
using Crane.Core.Api;
using log4net;

namespace Crane.PowerShell
{
    [System.Management.Automation.Cmdlet(VerbsCommon.Get, "CraneSolutionContext"), OutputType(typeof(ISolutionContext))]
    public class GetCraneSolutionContext : CraneCmdlet
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(GetCraneSolutionContext));
        
        [Parameter(Position = 0, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, Mandatory = true)]
        public string Path
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
            WriteObject(Api.GetSolutionContext(Path));                        
        }
    }
}