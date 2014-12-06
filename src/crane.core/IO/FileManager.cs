using System.Collections.Generic;
using System.IO;
using log4net;

namespace Crane.Core.IO
{
    public class FileManager : IFileManager
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(FileManager));

        public void CopyFiles(string sourcePath, string destinationPath, bool copySubDirectories)
        {
            // Get the subdirectories for the specified directory.
            var directory = new DirectoryInfo(sourcePath);
            var childDirectories = directory.GetDirectories();

            if (!directory.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourcePath);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = directory.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destinationPath, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirectories)
            {
                foreach (DirectoryInfo subdir in childDirectories)
                {
                    string temppath = Path.Combine(destinationPath, subdir.Name);
                    CopyFiles(subdir.FullName, temppath, copySubDirectories);
                }
            }       
        }

        public string CurrentDirectory
        {
            get { return System.Environment.CurrentDirectory; }
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public string GetTemporaryDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        public void RenameFile(string path, string name)
        {
            File.Move(path, Path.Combine(new FileInfo(path).DirectoryName, name));
        }

        public void Delete(DirectoryInfo directory)
        {
            Directory.Delete(directory.FullName, true);
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

        public IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateDirectories(path, searchPattern, searchOption);
        }
    }
}