using Crane.Core.Configuration;
using Crane.Core.Templates.Psake;

namespace Crane.Core.Templates.Resolvers
{
    public class TemplateResolver : ITemplateResolver
    {
        private readonly IConfiguration _configuration;

        public TemplateResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ITemplate Resolve(TemplateType build)
        {
            return new PsakeBuildTemplate();
        }
    }
}