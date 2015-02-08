using System.IO;

namespace Crane.Core.Api.Model
{
    public class ProjectFile
    {
        private string _include;
        private FileInfo _fileInfo;

        public string Name
        {
            get
            {
                return FileInfo.Name;
            }
        }

        public string Include
        {
            get { return _include; }
            set
            {
                _include = value;                          
            }
        }

        public string Path
        {
            get
            {
                return FileInfo.FullName;
            }
        }

        public string RootDirectory { get; set; }

        private FileInfo FileInfo
        {
            get
            {
                if (_fileInfo == null)
                {
                    _fileInfo = new FileInfo(System.IO.Path.Combine(RootDirectory, Include));
                }      

                return _fileInfo;
            }
        }
    }
}