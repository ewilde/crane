using System.Collections.Generic;
using System.IO;
using log4net;

namespace Crane.Core.IO
{
    public class FileManager : IFileManager
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(FileManager));

        public void CopyFiles(string sourcePath, string destinationPath, string filter)
        {
            foreach (var file in EnumerateFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                var destFileName = Path.Combine(destinationPath, new FileInfo(file).Name);
                _log.DebugFormat("Copying file from {0} to {1}.", file, destFileName);
                File.Copy(file, destFileName);
            }            
        }

        public string CurrentDirectory
        {
            get { return System.Environment.CurrentDirectory; }
        }

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);            
        }

        public void RenameDirectory(string path, string name)
        {
            Directory.Move(path, Path.Combine(new DirectoryInfo(path).Parent.FullName, name));
        }

        public void WriteAllText(string path, string text)
        {
            File.WriteAllText(path, text);
        }

        public IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateFiles(path, searchPattern, searchOption);
        }
    }
}