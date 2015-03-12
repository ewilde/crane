using System.Collections;
using System.Collections.Generic;
using Crane.Core.Api.Exceptions;
using Crane.Core.Api.Model;
using Crane.Core.Runners;

namespace Crane.Core.Api
{
    public interface ICraneApi
    {
        ISolutionContext GetSolutionContext(string rootFolderPath);

        void PatchAssemblyInfo(AssemblyInfo assemblyInfo);

        void PatchSolutionAssemblyInfo(ISolutionContext solutionContext, string version);

        ISourceControlInformation GetSourceControlInformation(ISolutionContext solutionContext);
        
        /// <summary>
        /// Publish all the nuget specification files found in the supplied <paramref name="solutionContext"/>.
        /// </summary>
        /// <param name="solutionContext">The current solution context</param>
        /// <param name="nugetOutputPath">Directory containing the </param>
        /// <param name="version">Package version</param>
        /// <param name="source">Nuget api uri</param>
        /// <param name="apiKey">Nuget api key</param>
        /// <returns>A list of the nuget push command results</returns>
        /// <exception cref="NuGetException">if the command fails.</exception>
        IEnumerable<RunResult> NugetPublish(ISolutionContext solutionContext, string nugetOutputPath, string version, string source, string apiKey);
        
        /// <summary>
        /// Package all the nuget specification files found in the supplied <paramref name="solutionContext"/>.
        /// </summary>
        /// <param name="solutionContext">The current solution context</param>
        /// <param name="buildOutputPath">The redirected build output path this will be supplied to the nuget specification file as the variable build_output</param>
        /// <param name="nugetOutputPath">Output directory to place the build nuget packages</param>
        /// <param name="version">Package version, this will be supplied to the nuget specification file as the variable version_number.</param>
        /// <returns>A list of the nuget pack command results</returns>
        /// <exception cref="NuGetException">if the command fails.</exception>
        IEnumerable<RunResult> NugetPack(ISolutionContext solutionContext, string buildOutputPath, string nugetOutputPath, string version);

        /// <summary>
        /// Create a chocolatey package for the supplied <paramref name="chocolateySpecPath">chocolatey specification file</paramref>.
        /// </summary>
        /// <param name="solutionContext">The current solution context</param>
        /// <param name="buildOutputPath">The redirected build output path this will be supplied to the nuget specification file as the variable build_output</param>
        /// <param name="chocolateySpecPath">The full path to the chocolatey specification file to package</param>
        /// <param name="chocolateyOutputPath">Output directory to place the build chocolatey package</param>
        /// <param name="version">Package version, this will be supplied to the nuget specification file as the variable version_number.</param>
        /// <returns>Result of the chocolatey package command</returns>
        /// <exception cref="ChoclateyException">if the command fails.</exception>
        RunResult ChocolateyPack(ISolutionContext solutionContext, string chocolateySpecPath, string buildOutputPath, string chocolateyOutputPath, string version);
    }
}