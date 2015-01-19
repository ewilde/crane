using System.Collections.Generic;

namespace Crane.Core.Api
{
    public class SolutionContext : ISolutionContext
    {
        public string Path { get; set; }

        public Solution Solution { get; set; }
    }
}