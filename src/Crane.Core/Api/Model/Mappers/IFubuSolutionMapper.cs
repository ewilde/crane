using System;

namespace Crane.Core.Api.Model.Mappers
{
    public interface IFubuSolutionMapper
    {
        Solution Map(FubuCsProjFile.Solution solution);
    }
}