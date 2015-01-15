using System.Collections.Generic;

namespace Crane.Core.Api
{
    public interface ISolutionContext
    {
        IEnumerable<Project> Projects { get; set; }
    }
}