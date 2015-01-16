using System;

namespace Crane.Core.Api.Builders
{
    public interface ISolutionBuilder
    {
        ISolutionBuilder WithProject(Action<Project> assign);

        ISolutionBuilder WithSolution(Action<Solution> assign);

        ISolutionContext Build();
    }
}