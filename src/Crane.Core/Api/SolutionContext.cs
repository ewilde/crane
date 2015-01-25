using System.Collections.Generic;
using Crane.Core.Api.Model;

namespace Crane.Core.Api
{
    public class SolutionContext : ISolutionContext
    {
        public string Path { get; set; }

        public Solution Solution { get; set; }
    }
}