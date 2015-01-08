using System;
using System.Collections.Generic;
using System.IO;
using Crane.Core.Configuration;
using Crane.Core.Extensions;
using Crane.Core.IO;

namespace Crane.Core.Templates.Resolvers
{
    public class TemplateLoader : ITemplateLoader
    {
        private readonly IFileManager _fileManager;
        private readonly ICraneContext _context;
        private readonly ITemplateFactory _templateFactory;

        public TemplateLoader(IFileManager fileManager, ICraneContext context, ITemplateFactory templateFactory)
        {
            _fileManager = fileManager;
            _context = context;
            _templateFactory = templateFactory;
        }

        public IEnumerable<ITemplate> Load()
        {
            var folders = new[]
            {
                new Tuple<string, TemplateType>("build", TemplateType.Build),
                new Tuple<string, TemplateType>("source", TemplateType.Source)
            };

            var result = new List<ITemplate>();
            folders.ForEach(
                item =>
                    _fileManager.EnumerateDirectories(Path.Combine(_context.TemplateDirectory.FullName, item.Item1),
                        "*.*", SearchOption.TopDirectoryOnly).ForEach(
                            directory => result.Add(_templateFactory.Create(new DirectoryInfo(directory), item.Item2))));
            return result;
        }
        

        
    }
}