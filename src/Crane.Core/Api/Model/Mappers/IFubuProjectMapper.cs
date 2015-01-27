using System;
using Crane.Core.Api.Model;

namespace Crane.Core.Api.Mappers
{
    public interface IFubuProjectMapper
    {
        Project Map(FubuCsProjFile.CsProjFile project);
    }
}