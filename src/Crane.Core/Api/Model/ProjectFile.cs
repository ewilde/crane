namespace Crane.Core.Api.Model
{
    public class ProjectFile
    {
        public string Include { get; set; }

        public string Path
        {
            get { return System.IO.Path.Combine(RootDirectory, Include); }
        }

        public string RootDirectory { get; set; }
    }
}