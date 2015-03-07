using System;
using System.Management.Automation;
using Crane.Core.Api;
using Crane.Core.Runners;
using log4net;

namespace Crane.PowerShell
{
    [Cmdlet(VerbsLifecycle.Invoke, "CraneNugetPackAllProjects"), OutputType(new Type[] {typeof(RunResult)})]
    public class InvokeCraneNugetPackAllProjects : CraneCmdlet
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(InvokeCraneNugetPackAllProjects));

        [Parameter(Position = 0, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, Mandatory = true)]
        public ISolutionContext SolutionContext
        {
            get;
            set;
        }

        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, Mandatory = true)]
        public string BuildOutputPath
        {
            get;
            set;
        }

        [Parameter(Position = 2, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, Mandatory = true)]
        public string NugetOutputPath
        {
            get;
            set;
        }

        [Parameter(Position = 3, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, Mandatory = true)]
        public string Version
        {
            get;
            set;
        }

        protected override ILog Log
        {
            get { return _log; }
        }

        public ICraneApi Api { get; set; }

        internal override void Process()
        {
            foreach(var item in Api.NugetPack(SolutionContext, BuildOutputPath, NugetOutputPath, Version))
            {
                WriteObject(item);
            }
        }
    }
}