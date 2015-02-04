using System.Collections.Generic;
using System.Linq;

namespace Crane.Core.Api.Model
{
    public class Solution
    {
        public IEnumerable<Project> Projects { get; set; }

        public IEnumerable<Project> CodeProjects { get { return Projects.Where(p => !p.TestProject); } } 

        /// <summary>
        /// Name of the solution
        /// </summary>
        /// <example>sallyfx</example>
        public string Name { get; set; }

        /// <summary>
        /// Full path to the solution file
        /// </summary>
        /// <example>c:\dev\sallyfx\sallyfx.sln</example>
        public string Path { get; set; }

        public ISolutionContext SolutionContext { get; set; }
    }
}