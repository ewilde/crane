using System.Collections.Generic;
using Crane.Core.Api.Model;

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