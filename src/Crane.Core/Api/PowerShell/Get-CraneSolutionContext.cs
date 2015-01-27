using System;
using System.Management.Automation;
using log4net;

namespace Crane.Core.Api.PowerShell
{
    [System.Management.Automation.Cmdlet(VerbsCommon.Get, "CraneSolutionContext"), OutputType(typeof(ISolutionContext))]
    public class GetCraneSolutionContext : CraneCmdlet
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(GetCraneSolutionContext));

        public GetCraneSolutionContext()
        {
        }

        [Parameter(Position = 0, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, Mandatory = true)]
        public string Path
        {
            get;
            set;
        }

        protected override ILog Log
        {
            get { return _log; }
        }

        internal override void Process()
        {
                        
        }
    }
}