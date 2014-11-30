using System;
using System.Collections.Generic;
using System.Linq;
using Crane.Core.Configuration;
using Crane.Core.Templates.Psake;

namespace Crane.Core.Templates.Resolvers
{
    public class TemplateResolver : ITemplateResolver
    {
        private readonly IConfiguration _configuration;

        public TemplateResolver(IConfiguration configuration, IEnumerable<ITemplate> templates)
        {
            _configuration = configuration;
            Templates = templates;
        }

        public IEnumerable<ITemplate> Templates { get; set; }

        public ITemplate Resolve(TemplateType templateType)
        {
            return
                Templates.FirstOrDefault(
                    item => item.Name.Equals(_configuration.BuildTemplateProviderName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}