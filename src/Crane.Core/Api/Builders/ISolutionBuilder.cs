using System;

namespace Crane.Core.Api.Builders
{
    public interface ISolutionBuilder
    {
        string RootPath { get; set; }
        ISolutionBuilder WithProject(Action<Project> assign);

        ISolutionContext Build();
    }
}