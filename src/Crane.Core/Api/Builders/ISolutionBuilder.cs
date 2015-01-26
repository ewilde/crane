using System;
using Crane.Core.Api.Model;

namespace Crane.Core.Api.Builders
{
    public interface ISolutionBuilder
    {
        ISolutionBuilder WithProject(Action<Project> assign);

        ISolutionBuilder WithSolution(Action<Solution> assign);

        ISolutionContext Build();
        ISolutionBuilder WithFile<T>(Action<T> assign) where T : ProjectFile, new();

        ISolutionBuilder WithFile(Action<PlainFile> assign);
    }
}