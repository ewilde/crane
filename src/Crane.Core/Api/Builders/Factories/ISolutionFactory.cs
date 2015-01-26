using System.Collections.Generic;
using Crane.Core.Api.Model;

namespace Crane.Core.Api.Builders
{
    public interface ISolutionFactory
    {
        /// <summary>
        /// Create a new solution
        /// </summary>
        /// <param name="fullName">Full path to the solution to create. i.e. c:\dev\Sally\Sally.sln</param>
        /// <param name="projects">Projects that make up the new solution</param>
        Solution Create(string fullName, IEnumerable<Project> projects);        
    }
}