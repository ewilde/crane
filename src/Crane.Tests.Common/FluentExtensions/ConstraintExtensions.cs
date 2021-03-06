using Crane.Core.Runners;
using Crane.Tests.Common.Runners;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Crane.Tests.Common.FluentExtensions
{
    public static class ConstraintExtensions
    {
        public static AndConstraint<GenericAssertions<RunResult>> BeErrorFree(this GenericAssertions<RunResult> value,
            string because = "", params object[] reasonArgs)
        {
            using (var assertionScope = Execute.Assertion)
            {
                assertionScope.ForCondition(
                    !value.Subject.StandardOutput.ToLower().Contains("error") &&
                    !value.Subject.StandardOutput.ToLower().Contains("invalid") &&
                    value.Subject.ErrorOutput.Trim().Length == 0 &&
                    value.Subject.ExitCode == 0)
                    .BecauseOf(because, reasonArgs).FailWith(" Expected not to contain errors{reason}, but found {0}.", new object[1]
                    {
                        value.Subject
                    });
            }

            return new AndConstraint<GenericAssertions<RunResult>>(value);
        }

        public static AndConstraint<GenericAssertions<RunResult>> BeBuildSuccessful(this GenericAssertions<RunResult> value,
            string because = "", params object[] reasonArgs)
        {
            using (var assertionScope = Execute.Assertion)
            {
                assertionScope.ForCondition(
                    value.Subject.StandardOutput.Contains("Succeeded!")).BecauseOf(because, reasonArgs).FailWith(" Expected not to contain errors{reason}, but found {0}.", new object[1]
                    {
                        value.Subject
                    });
            }

            return new AndConstraint<GenericAssertions<RunResult>>(value);
        }

        public static AndConstraint<GenericAssertions<RunResult>> BeBuiltSuccessfulyWithAllTestsPassing(
            this GenericAssertions<RunResult> value,
            string because = "", params object[] reasonArgs)
        {
            value.BeBuildSuccessful(because, reasonArgs).And.HaveAllTestsRunAndPass(because, reasonArgs);
            return new AndConstraint<GenericAssertions<RunResult>>(value);
        }

        public static AndConstraint<GenericAssertions<RunResult>> HaveAllTestsRunAndPass(
            this GenericAssertions<RunResult> value,
            string because = "", params object[] reasonArgs)
        {
            using (var assertionScope = Execute.Assertion)
            {
                assertionScope.ForCondition(
                    value.Subject.StandardOutput.Contains(", 0 failed") ||
                    !value.Subject.StandardOutput.Contains("[testFailed")).BecauseOf(because, reasonArgs).FailWith(" Expected not to contain errors{reason}, but found {0}.", new object[1]
                    {
                        value.Subject
                    });
            }

            return new AndConstraint<GenericAssertions<RunResult>>(value);
        }
    }
}