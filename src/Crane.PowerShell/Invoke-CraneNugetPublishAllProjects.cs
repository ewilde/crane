using System;
using System.Management.Automation;
using Crane.Core.Api;
using Crane.Core.Runners;
using log4net;

namespace Crane.PowerShell
{
    [Cmdlet(VerbsLifecycle.Invoke, "CraneNugetPublishAllProjects"), OutputType(new Type[] { typeof(RunResult) })]
    public class InvokeCraneNugetPublishAllProjects : CraneCmdlet
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(InvokeCraneNugetPublishAllProjects));

        [Parameter(Position = 0, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, Mandatory = true)]
        public ISolutionContext SolutionContext
        {
            get;
            set;
        }

        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, Mandatory = true)]
        public string NugetOutputPath
        {
            get;
            set;
        }

        [Parameter(Position = 2, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, Mandatory = true)]
        public string Version
        {
            get;
            set;
        }

        [Parameter(Position = 3, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, Mandatory = true)]
        public string Source
        {
            get;
            set;
        }

        [Parameter(Position = 4, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, Mandatory = true)]
        public string ApiKey
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
            foreach(var item in Api.NugetPublish(SolutionContext, NugetOutputPath, Version, Source, ApiKey))
            {
                WriteObject(item);
            }
        }
    }
}