using System;
using System.IO;
using Crane.Core.Api.Mappers;
using Crane.Core.IO;
using FubuCsProjFile;

namespace Crane.Core.Api.Model.Mappers
{
    public class FubuProjectMapper : IFubuProjectMapper
    {
        private readonly IFileManager _fileManager;

        public FubuProjectMapper(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }

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
                        Version = info.AssemblyVersion//,
                        //InformationalVersion = info.AssemblyInformationalVersion
                    };
                }
                else
                {
                    mapped = new ProjectFile();
                }

                mapped.Include = _fileManager.GetPathForHostEnvironment(item.Include);
                mapped.RootDirectory = _fileManager.GetPathForHostEnvironment(result.Directory);

                result.Files.Add(mapped);
            }

            return result;
        }
    }
}