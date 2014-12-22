using FluentAssertions;
using Xbehave;

namespace %context.ProjectName%.UnitTests
{
    public class CalculatorFeature
    {
        [Scenario]
        public void adding_two_whole_numbers_returns_correct_result(Calculator calculator, int result)
        {
            "Given an instance of calculator"
                ._(() => calculator = new Calculator());

            "When I add two numbers"
                ._(() => result = calculator.Add(10, 14));

            "Then the result should be correct"
                ._(() => result.Should().Be(24));
        }
    }
}
