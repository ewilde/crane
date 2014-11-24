using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FubuCsProjFile;

namespace crane.core.tests.Data
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
