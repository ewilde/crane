using System.Collections.Generic;

namespace Crane.Core.Api
{
    public class Solution
    {
        public IEnumerable<Project> Projects { get; set; }

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
    }
}