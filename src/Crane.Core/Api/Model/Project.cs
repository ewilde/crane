namespace Crane.Core.Api
{
    public class Project
    {
        public string Name { get;  set; }

        public string Path { get; set; }

        public ProjectFile AssemblyInfo { get; set; }

        public Solution Solution { get; set; }
    }
}