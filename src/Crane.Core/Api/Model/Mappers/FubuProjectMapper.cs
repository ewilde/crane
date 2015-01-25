using System;
using System.IO;
using Crane.Core.Api.Mappers;
using FubuCsProjFile;

namespace Crane.Core.Api.Model.Mappers
{
    [CLSCompliant(false)]
    public class FubuProjectMapper : IFubuProjectMapper
    {
        public Project Map(CsProjFile project)
        {
            var result = new Project
            {
                Name = project.ProjectName,
                Path = Path.GetFullPath(project.FileName)
            };

            foreach (var item in project.All<CodeFile>())
            {
                ProjectFile mapped;
                if (item.Include.EndsWith("AssemblyInfo.cs"))
                {
                    var info = project.AssemblyInfo;
                    mapped = new AssemblyInfo()
                    {
                        Title = info.AssemblyTitle,
                        Description = info.AssemblyDescription,
                        FileVersion = info.AssemblyFileVersion,
                        Version = info.AssemblyVersion,
                        InformationalVersion = info.AssemblyInformationalVersionAttribute
                    };
                }
                else
                {
                    mapped = new ProjectFile();
                }

                mapped.Include = item.Include;
                mapped.RootDirectory = result.Directory;

                result.Files.Add(mapped);
            }

            return result;
        }
    }
}