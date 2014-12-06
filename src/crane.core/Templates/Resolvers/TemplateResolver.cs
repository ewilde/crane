using System;
using System.Collections.Generic;
using System.Linq;
using Crane.Core.Configuration;

namespace Crane.Core.Templates.Resolvers
{
    public class TemplateResolver : ITemplateResolver
    {
        private readonly IConfiguration _configuration;
        private readonly ITemplateLoader _loader;

        public TemplateResolver(IConfiguration configuration, ITemplateLoader loader)
        {
            _configuration = configuration;
            _loader = loader;
            Templates = loader.Load();
        }

        public IEnumerable<ITemplate> Templates { get; set; }

        public ITemplate Resolve(TemplateType templateType)
        {
            var name = string.Empty;

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

            return
                Templates.FirstOrDefault(
                    item => item.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}