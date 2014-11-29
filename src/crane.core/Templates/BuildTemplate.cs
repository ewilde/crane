using System.Collections.Generic;
using System.IO;

namespace Crane.Core.Templates
{
    public abstract class BuildTemplate : IBuildTemplate
    {
        protected abstract void CreateCore();
        
        public abstract string Name { get; }

        public abstract DirectoryInfo TemplateSourceDirectory { get; set; }
        
        public abstract FileInfo BuildScript { get; }

        protected abstract IEnumerable<FileInfo> TemplatedFiles { get; }

        public void Create()
        {
            this.CreateCore();
        }
    }
}