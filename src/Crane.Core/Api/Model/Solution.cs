using System.Collections.Generic;

namespace Crane.Core.Api
{
    public class Solution
    {
        public IEnumerable<Project> Projects { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }
    }
}