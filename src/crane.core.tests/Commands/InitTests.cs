using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crane.Core.Commands;
using Crane.Core.Configuration;
using Crane.Core.IO;
using Crane.Core.Templates;
using Crane.Core.Templates.Resolvers;
using FakeItEasy;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Commands
{
    public class InitTests
    {
        //[Scenario]
        //public void calling_execute_to_create_a_blank_project(ICraneContext craneContext, MockContainer<Init> init)
        //{
        //    var buildTemplate = A.Fake<ITemplate>();

        //    "Given I have an instance of the init command"
        //        ._(() => init = B.AutoMock<Init>());

        //    "And I am in the working directory dev"
        //        ._(() => A.CallTo(() => init.GetMock<IFileManager>().CurrentDirectory).Returns(@"c:\dev"));

        //    "And the template resolver is configured to return at least a build template"
        //        ._(() => A.CallTo(() => init.GetMock<ITemplateResolver>().Resolve(TemplateType.Build)).Returns(buildTemplate));
            
        //    "When I call execute with a project name"
        //        ._(() => init.Subject.Execute("FooStack"));

        //    "It sets the project directory to the current directory"
        //        ._(() => init.GetMock<ICraneContext>().ProjectRootDirectory.FullName.Should().Be(@"c:\dev\FooStack"));

        //    "It creates the project directory"
        //        ._(() => A.CallTo(() => init.GetMock<IFileManager>().CreateDirectory(@"c:\dev\FooStack")).MustHaveHappened());

        //    "It creates a new build script for that project"
        //        ._(() => A.CallTo(() => buildTemplate.Create()).MustHaveHappened());
        //}
    }
}
