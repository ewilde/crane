using Crane.Core.Commands;
using Crane.Core.Commands.Execution;
using Crane.Core.Commands.Resolvers;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Commands.Execution
{
    public class DidYouMeanExecutorTests
    {

        private class Construct : ICraneCommand
        {
            public void Execute() { }
        }

        [Scenario]
        public void Execute_logs_did_you_mean_text_correctly_for_no_args(DidYouMeanExecutor didYouMeanExecutor, string result)
        {
            "Given I have a didYouMeanExecutor".
            _(() => didYouMeanExecutor = new DidYouMeanExecutor(new ClosestCommandMethodResolver(), s => result = s));

            "When I PrintHelp"
                ._(() => didYouMeanExecutor.PrintHelp(new Construct(), new string[0]));

            "Then the output is did you mean 'crane construct'?"
                ._(() => result.Should().Be("did you mean 'crane construct'?"));
        }

        private class Build : ICraneCommand
        {
            public void Execute(string project) { }
        }

        [Scenario]
        public void  Execute_logs_did_you_mean_text_correctly_for_one_arg(DidYouMeanExecutor didYouMeanExecutor, string result)
        {
            "Given I have a didYouMeanExecutor".
            _(() => didYouMeanExecutor = new DidYouMeanExecutor(new ClosestCommandMethodResolver(), s => result = s));

            "When I PrintHelp"
                ._(() => didYouMeanExecutor.PrintHelp(new Build(), new string[0]));

            "Then the output is did you mean 'crane build project'?"
                ._(() => result.Should().Be("did you mean 'crane build project'?"));
        }

        private class Hoist : ICraneCommand
        {
            public void Execute(string from, string to) { }
        }

        [Scenario]
        public void Execute_logs_did_you_mean_text_correctly_for_two_args(DidYouMeanExecutor didYouMeanExecutor, string result)
        {
            "Given I have a didYouMeanExecutor".
            _(() => didYouMeanExecutor = new DidYouMeanExecutor(new ClosestCommandMethodResolver(), s => result = s));

            "When I PrintHelp"
                ._(() => didYouMeanExecutor.PrintHelp(new Hoist(), new string[0]));

            "Then the output is did you mean 'crane hoist from to'?"
                ._(() => result.Should().Be("did you mean 'crane hoist from to'?"));
        }
    }
}
