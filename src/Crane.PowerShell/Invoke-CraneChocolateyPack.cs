using System;
using System.Management.Automation;
using Crane.Core.Api;
using Crane.Core.Runners;
using log4net;

namespace Crane.PowerShell
{
    [Cmdlet(VerbsLifecycle.Invoke, "CraneChocolateyPack "), OutputType(new Type[] { typeof(RunResult) })]
    public class InvokeCraneChocolateyPack : CraneCmdlet
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(InvokeCraneChocolateyPack));

        [Parameter(Position = 0, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, Mandatory = true)]
        public ISolutionContext SolutionContext
        {
            get;
            set;
        }

        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, Mandatory = true)]
        public string ChocolateySpecPath
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
            WriteObject(Api.ChocolateyPack(SolutionContext, ChocolateySpecPath));
        }
    }
}