using System.Collections.Generic;
using System.IO;
using Crane.Core.Configuration;
using Crane.Core.IO;

namespace Crane.Core.Templates
{
    public abstract class BuildTemplate : IBuildTemplate
    {
        private readonly IFileManager _fileManager;
        private readonly ITemplateParser _templateParser;
        private readonly ICraneContext _context;
        private readonly IConfiguration _configuration;

        public BuildTemplate(ICraneContext context, IConfiguration configuration, IFileManager fileManager, ITemplateParser templateParser)
        {
            _fileManager = fileManager;
            _templateParser = templateParser;
            _context = context;
            _configuration = configuration;
        }
            
        protected abstract void CreateCore();
        
        public abstract string Name { get; }

        public abstract DirectoryInfo TemplateSourceDirectory { get; set; }
        
        public abstract FileInfo BuildScript { get; }

        protected abstract IEnumerable<FileInfo> TemplatedFiles { get; }

        protected IFileManager FileManager
        {
            get { return _fileManager; }
        }

        protected ICraneContext Context
        {
            get { return _context; }
        }

        protected IConfiguration Configuration
        {
            get { return _configuration; }
        }

        public void Create()
        {
            this.CreateCore();
            this.ParseTemplate();
        }

        protected virtual void ParseTemplate()
        {
            foreach (var file in this.TemplatedFiles)
            {
                var parsed = _templateParser.Parse(file, this.Context);
                FileManager.WriteAllText(file.FullName, parsed);
            }
        }
    }
}