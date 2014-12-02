using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Crane.Core.Commands;
using Crane.Core.Configuration.Modules;
using Crane.Core.Templates.Parsers;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Configuration.Modules
{
    public class TemplateModuleTests
    {
        [Scenario]
        public void By_default_token_parser_is_the_configured_template_parser(IContainer container)
        {
            "Given we have bootstraped the IoC"
                ._(() => container = BootStrap.Start());

            "Then it should resolve the token template parser"
                ._(() => container.Resolve<ITemplateParser>().Should().BeOfType<TokenTemplateParser>());
        }
    }
}
