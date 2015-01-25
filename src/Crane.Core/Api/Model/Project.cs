using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                return new FileInfo(this.Path).DirectoryName;
            }
        }

        public AssemblyInfo AssemblyInfo
        {
            get { return (AssemblyInfo) this.Files.FirstOrDefault(item => item is AssemblyInfo); }
        }

        public Solution Solution { get; set; }

        public IList<ProjectFile> Files { get; set; }
    }
}