using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Crane.Core.Extensions;

namespace Crane.Core.Api.Model
{
    public class Project
    {
        public Project()
        {
            Files = new List<ProjectFile>();
        }

        public string Name { get;  set; }

        public string Path { get; set; }

        public string Directory
        {
            get
            {
                var directory = new FileInfo(this.Path).DirectoryName;
                if (directory == null)
                {
                    throw new DirectoryNotFoundException(string.Format("Could not find directory for path {0} in project {1}.", Path, Name));    
                }

                return directory;
            }
        }

        public AssemblyInfo AssemblyInfo
        {
            get { return (AssemblyInfo) this.Files.FirstOrDefault(item => item is AssemblyInfo); }
        }

        public Solution Solution { get; set; }

        public IList<ProjectFile> Files { get; set; }

        public bool TestProject
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Name))
                {
                    return false;
                }
                return Name.Contains("Tests", StringComparison.InvariantCultureIgnoreCase);
            }
        }

        public ProjectFile NugetSpec
        {
            get
            {
                var name = string.Format("{0}.{1}", Name, "nuspec");
                var path = System.IO.Path.Combine(Directory, name);
                return File.Exists(path) ? new ProjectFile {Include = name, RootDirectory = Directory} : null;
            }
        }
    }
}