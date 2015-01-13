using System.IO;
using Crane.Core.Configuration;

namespace Crane.Core.Templates.Resolvers
{
    public class TemplateFactory : ITemplateFactory
    {
        private readonly IConfiguration _configuration;

        public TemplateFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ITemplate Create(DirectoryInfo directoryInfo, TemplateType templateType)
        {
            var template = new Template
            {
                Name = directoryInfo.Name,
                TemplateType = templateType,
                TemplateSourceDirectory = directoryInfo,
                InstallFolderRootName = templateType == TemplateType.Build ? _configuration.BuildFolderName : _configuration.SourceFolderName
            };

            return template;

        }
    }
}