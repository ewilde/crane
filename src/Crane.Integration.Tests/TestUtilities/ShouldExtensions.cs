using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace Crane.Integration.Tests.TestUtilities
{
    public static class ShouldExtensions
    {
        public static AndConstraint<StringAssertions> BeCraneOutputErrorFree(this StringAssertions value, string because = "", params object[] reasonArgs)
        {
            Execute.Assertion.ForCondition(value.Subject != null && !value.Subject.ToLower().Contains("error")).BecauseOf(because, reasonArgs).FailWith(" Expected not to contain errors{reason}, but found {0}.", new object[1]
              {
                (object) value.Subject
              });
            return new AndConstraint<StringAssertions>(value);
        }
    }
}