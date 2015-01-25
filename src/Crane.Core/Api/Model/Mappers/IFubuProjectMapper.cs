using System;
using Crane.Core.Api.Model;

namespace Crane.Core.Api.Mappers
{
    [CLSCompliant(false)]
    public interface IFubuProjectMapper
    {
        Project Map(FubuCsProjFile.CsProjFile project);
    }
}