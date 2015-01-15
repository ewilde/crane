using System;

namespace Crane.Core.Api.Builders
{
    public class SolutionBuilderFactory : ISolutionBuilderFactory
    {
        private readonly Func<ISolutionBuilder> _activator;

        public SolutionBuilderFactory(Func<ISolutionBuilder> activator)
        {
            _activator = activator;
        }

        public ISolutionBuilder Create(string rootPath)
        {
            var result = _activator.Invoke();
            result.RootPath = rootPath;
            return result; 
        }
    }
}