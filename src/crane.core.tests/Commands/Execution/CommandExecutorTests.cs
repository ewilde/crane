using Crane.Core.Commands;
using Crane.Core.Commands.Execution;
using Crane.Core.Commands.Resolvers;
using Crane.Core.IO;
using FakeItEasy;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Commands.Execution
{
    public class CommandExecutorTests
    {
        public class Test : ICraneCommand
        {
            public string Name { get { return "Test"; } }

            public bool NoArgsExecuted { get; private set; }
            public bool OneArgExecuted { get; private set; }
            public bool TwoArgsExecuted { get; private set; }
            public bool ThreeArgsExecuted { get; private set; }

            public string[] Args { get; private set; }

            public Test()
            {
                NoArgsExecuted = OneArgExecuted = TwoArgsExecuted = ThreeArgsExecuted = false;
            }

            public void NoArgs()
            {
                NoArgsExecuted = true;
                Args = new string[0];
            }

            public void OneArg(string arg1)
            {
                OneArgExecuted = true;
                Args = new[] {arg1};
            }

            public void TwoArgs(string arg1, string arg2)
            {
                TwoArgsExecuted = true;
                Args = new[] {arg1, arg2};
            }

            public void ThreeArgs(string arg1, string arg2, string arg3)
            {
                ThreeArgsExecuted = true;
                Args = new[] {arg1, arg2, arg3};
            }

        }

        [Scenario]
        public void Run_test_with_no_arguments_executes_correct_method(CommandExecutor commandExecutor, Test test)
        {
            "Given I have a test command"
                ._(() =>
                {
                    test = new Test();
                    commandExecutor = new CommandExecutor(new ICraneCommand[] { B.AutoMock<Init>().Subject, test }, new CommandResolver(), new CommandMethodResolver(), A.Fake<IDidYouMeanExecutor>(), A.Fake<IOutput>());
                });

            "When I execute 'test'"
                ._(() => commandExecutor.ExecuteCommand(new[] {"test"}));

            "Then the method with no arguments is executed on the test command"
                ._(() => test.NoArgsExecuted.Should().BeTrue());

            "And no arguments should be passed in"
                ._(() => test.Args.Length.Should().Be(0));

            "And the method with 1 argument should not be executed"
                ._(() => test.OneArgExecuted.Should().BeFalse());

            "And the method with 2 arguments should not be executed"
                ._(() => test.TwoArgsExecuted.Should().BeFalse());

            "And the method with 3 arguments should not be executed"
                ._(() => test.ThreeArgsExecuted.Should().BeFalse());

        }

        [Scenario]
        public void Run_test_with_one_argument_executes_correct_method(CommandExecutor commandExecutor, Test test)
        {
            "Given I have a test command"
                ._(() =>
                {
                    test = new Test();
                    commandExecutor = new CommandExecutor(new ICraneCommand[] { B.AutoMock<Init>().Subject, test }, new CommandResolver(), new CommandMethodResolver(), A.Fake<IDidYouMeanExecutor>(), A.Fake<IOutput>());
                });

            "When I execute 'test firstarg'"
                ._(() => commandExecutor.ExecuteCommand(new[] { "test", "firstarg" }));

            "Then the method with no arguments is not executed on the test command"
                ._(() => test.NoArgsExecuted.Should().BeFalse());

            "And one argument should be passed in"
                ._(() => test.Args.Length.Should().Be(1));

            "And the argument should be firstarg"
                ._(() => test.Args[0].Should().Be("firstarg"));

            "And the method with 1 argument should be executed"
                ._(() => test.OneArgExecuted.Should().BeTrue());

            "And the method with 2 arguments should not be executed"
                ._(() => test.TwoArgsExecuted.Should().BeFalse());

            "And the method with 3 arguments should not be executed"
                ._(() => test.ThreeArgsExecuted.Should().BeFalse());

        }

        [Scenario]
        public void Run_test_with_two_arguments_executes_correct_method(CommandExecutor commandExecutor, Test test)
        {
            "Given I have a test command"
                ._(() =>
                {
                    test = new Test();
                    commandExecutor = new CommandExecutor(new ICraneCommand[] { B.AutoMock<Init>().Subject, test }, new CommandResolver(), new CommandMethodResolver(), A.Fake<IDidYouMeanExecutor>(), A.Fake<IOutput>());
                });

            "When I execute 'test firstarg secondarg'"
                ._(() => commandExecutor.ExecuteCommand(new[] { "test", "firstarg", "secondarg" }));

            "Then the method with no arguments is not executed on the test command"
                ._(() => test.NoArgsExecuted.Should().BeFalse());

            "And two arguments should be passed in"
                ._(() => test.Args.Length.Should().Be(2));

            "And the first argument should be firstarg"
                ._(() => test.Args[0].Should().Be("firstarg"));

            "And the second argument should be secondarg"
                ._(() => test.Args[1].Should().Be("secondarg"));

            "And the method with 1 argument should not be executed"
                ._(() => test.OneArgExecuted.Should().BeFalse());

            "And the method with 2 arguments should be executed"
                ._(() => test.TwoArgsExecuted.Should().BeTrue());

            "And the method with 3 arguments should not be executed"
                ._(() => test.ThreeArgsExecuted.Should().BeFalse());

        }

        [Scenario]
        public void Run_test_with_three_arguments_executes_correct_method(CommandExecutor commandExecutor, Test test)
        {
            "Given I have a test command"
                ._(() =>
                {
                    test = new Test();
                    commandExecutor = new CommandExecutor(new ICraneCommand[] { B.AutoMock<Init>().Subject, test }, new CommandResolver(), new CommandMethodResolver(), A.Fake<IDidYouMeanExecutor>(), A.Fake<IOutput>());
                });

            "When I execute 'test firstarg secondarg thridarg'"
                ._(() => commandExecutor.ExecuteCommand(new[] { "test", "firstarg", "secondarg", "thirdarg" }));

            "Then the method with no arguments is not executed on the test command"
                ._(() => test.NoArgsExecuted.Should().BeFalse());

            "And three arguments should be passed in"
                ._(() => test.Args.Length.Should().Be(3));

            "And the first argument should be firstarg"
                ._(() => test.Args[0].Should().Be("firstarg"));

            "And the second argument should be secondarg"
                ._(() => test.Args[1].Should().Be("secondarg"));

            "And the third argument should be secondarg"
                ._(() => test.Args[2].Should().Be("thirdarg"));

            "And the method with 1 argument should not be executed"
                ._(() => test.OneArgExecuted.Should().BeFalse());

            "And the method with 2 arguments should not be executed"
                ._(() => test.TwoArgsExecuted.Should().BeFalse());

            "And the method with 3 arguments should be executed"
                ._(() => test.ThreeArgsExecuted.Should().BeTrue());

        }


    }
}
