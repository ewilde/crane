using System.Collections.Generic;

namespace Crane.Core.Api
{
    public class SolutionContext : ISolutionContext
    {
        public IEnumerable<Project> Projects { get; set; }
    }
}