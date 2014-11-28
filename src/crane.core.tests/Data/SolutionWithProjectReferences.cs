using System;
using FubuCsProjFile;

namespace Crane.Core.Tests.Data
{
    public class SolutionWithProjectReferences
    {
        public Solution Create()
        {
            var solution = Solution.CreateNew(Environment.CurrentDirectory, "SolutionWithProjectReferences");

            var projectCore = new CsProjFile("project.core");
            var projectTest = new CsProjFile("project.core.tests");

            projectTest.Add<ProjectReference>(new ProjectReference());

            return solution;
        }
    }
}
