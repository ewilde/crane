using System;

namespace Crane.Core.Api.Builders
{
    public class SolutionBuilderFactory : ISolutionBuilderFactory
    {
        private readonly ISolutionBuilder _solutionBuilder;

        public SolutionBuilderFactory(ISolutionBuilder solutionBuilder)
        {
            _solutionBuilder = solutionBuilder;
        }

        public ISolutionBuilder Create()
        {
            return _solutionBuilder;
        }
    }
}