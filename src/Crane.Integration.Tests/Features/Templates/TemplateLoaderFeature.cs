using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crane.Core.Configuration;
using Crane.Core.IO;
using Crane.Core.Templates;
using Crane.Core.Templates.Resolvers;
using Crane.Integration.Tests.TestUtilities;
using Xbehave;

namespace Crane.Integration.Tests.Features.Templates
{
    public class TemplateLoaderFeature
    {
        [Scenario]
        public void should_load_default_templates_from_template_directory(
            ITemplateLoader templateLoader, DirectoryInfo root,
            ICraneContext context, IFileManager fileManager,
            IEnumerable<ITemplate> result)
        {            
            "Given I have a project root folder"
                ._(() =>
                {
                    context = ServiceLocator.Resolve<ICraneContext>(); fileManager = ServiceLocator.Resolve<IFileManager>();
                    context.ProjectRootDirectory = new DirectoryInfo(fileManager.GetTemporaryDirectory());
                });

            "And a template loader"
                ._(() => templateLoader = ServiceLocator.Resolve<ITemplateLoader>());

            "When I call load"
                ._(() => result = templateLoader.Load());

            "It should load the default source template"
                ._(
                    () =>
                        result.Any(
                            item =>
                                item.Name.Equals(CraneConfiguration.DefaultSourceProviderName) &&
                                item.TemplateType == TemplateType.Source));

            "It should load the default build template"
                ._(
                    () =>
                        result.Any(
                            item =>
                                item.Name.Equals(CraneConfiguration.DefaultBuildProviderName) &&
                                item.TemplateType == TemplateType.Build));
        }
    }
}
