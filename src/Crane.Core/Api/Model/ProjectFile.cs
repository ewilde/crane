namespace Crane.Core.Api.Model
{
    public class ProjectFile
    {
        private string _include;

        public string Include
        {
            get { return _include; }
            set { _include = value; }
        }

        public string Path
        {
            get { return System.IO.Path.Combine(RootDirectory, Include); }
        }

        public string RootDirectory { get; set; }
    }
}