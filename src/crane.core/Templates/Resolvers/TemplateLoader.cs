using System;
using System.Collections.Generic;
using System.IO;
using Crane.Core.Configuration;
using Crane.Core.IO;
using Crane.Core.Utility;

namespace Crane.Core.Templates.Resolvers
{
    public class TemplateLoader : ITemplateLoader
    {
        private readonly IFileManager _fileManager;
        private readonly ICraneContext _context;
        private readonly Func<ITemplate> _templateFactory;

        public TemplateLoader(IFileManager fileManager, ICraneContext context, Func<ITemplate> templateFactory)
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
                            directory => result.Add(CreateTemplateFromDirectory(directory, item.Item2))));
            return result;
        }

        private ITemplate CreateTemplateFromDirectory(string directory, TemplateType templateType)
        {
            var directoryInfo = new DirectoryInfo(directory);
            var template = _templateFactory.Invoke();
            template.Name = directoryInfo.Name;
            template.TemplateType = templateType;
            template.TemplateSourceDirectory = new DirectoryInfo(directory);
            return template;
        }
    }
}