using System.Collections.Generic;

namespace Crane.Core.Api
{
    public interface ISolutionContext
    {
        /// <summary>
        /// Root path to the top of the solution
        /// </summary>
        string Path { get; set; }

        /// <summary>
        /// The main solution associated with this project
        /// </summary>
        Solution Solution { get; set; }
    }
}