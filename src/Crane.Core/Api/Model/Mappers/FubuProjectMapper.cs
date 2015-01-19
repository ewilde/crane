using System;
using System.IO;
using FubuCsProjFile;

namespace Crane.Core.Api.Mappers
{
    [CLSCompliant(false)]
    public class FubuProjectMapper : IFubuProjectMapper
    {
        public Project Map(CsProjFile project)
        {
            return new Project
            {
                Name = project.ProjectName,
                Path = Path.GetFullPath(project.FileName)
            };
        }
    }
}