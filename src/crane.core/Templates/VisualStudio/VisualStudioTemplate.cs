using System.Collections.Generic;
using System.IO;
using Crane.Core.Configuration;
using Crane.Core.IO;
using Crane.Core.Templates.Parsers;

namespace Crane.Core.Templates.VisualStudio
{
    public class VisualStudioTemplate : BaseTemplate
    {
        public VisualStudioTemplate(ICraneContext context, IConfiguration configuration, IFileManager fileManager, ITemplateParser templateParser) :
            base(context, configuration, fileManager, templateParser)
        {
        }

        protected override void CreateCore()
        {
            var srcDir = Context.SourceDirectory.FullName;
            FileManager.CreateDirectory(srcDir);
            FileManager.CopyFiles(TemplateSourceDirectory.FullName, srcDir, "*.*");
        }

        public override string Name
        {
            get { return "VisualStudio"; }
        }

        protected override IEnumerable<FileInfo> TemplatedFiles
        {
            get
            {
                return new[]
                {
                    new FileInfo(Path.Combine(Context.SourceDirectory.FullName, string.Format("{0}.sln", Context.ProjectName)))
                };
            }
        }
    }
}