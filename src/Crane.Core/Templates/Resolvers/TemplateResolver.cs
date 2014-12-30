using System;
using System.Collections.Generic;
using System.Linq;
using Crane.Core.Commands.Exceptions;
using Crane.Core.Configuration;

namespace Crane.Core.Templates.Resolvers
{
    public class TemplateResolver : ITemplateResolver
    {
        private readonly IConfiguration _configuration;

        public TemplateResolver(IConfiguration configuration, ITemplateLoader loader)
        {
            _configuration = configuration;
            Templates = loader.Load();
        }

        public IEnumerable<ITemplate> Templates { get; set; }

        public ITemplate Resolve(TemplateType templateType)
        {
            string name;

            switch (templateType)
            {
               case TemplateType.Build:
                    name = _configuration.BuildTemplateProviderName;
                    break;
                case TemplateType.Source:
                    name = _configuration.SourceTemplateProviderName;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("templateType");
            }

            var template =
                Templates.FirstOrDefault(
                    item => item.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

            if (template == null)
            {
                throw new TemplateNotFoundCraneException(templateType);
            }

            return template;
        }
    }
}