using System;

namespace Crane.Core.Api.Model.Mappers
{
    [CLSCompliant(false)]
    public interface IFubuSolutionMapper
    {
        Solution Map(FubuCsProjFile.Solution solution);
    }
}