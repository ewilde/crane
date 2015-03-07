using System;
using System.Collections.Generic;

namespace Crane.Core.Runners
{
    public interface INuGet
    {
        RunResult Publish(string nuGetPackagePath,string source, string apiKey);

        RunResult Pack(string nuGetSpecPath, string outputDirectory, IEnumerable<Tuple<string, string>> properties);

        bool ValidateResult(RunResult result);
    }
}