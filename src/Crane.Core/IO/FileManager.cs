using System.Collections.Generic;
using System.IO;

namespace Crane.Core.IO
{
    public class FileManager : IFileManager
    {
        private readonly IHostEnvironment _hostEnvironment;

        public FileManager(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

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

        public bool FileExists(string path)
        {
            return File.Exists(path);
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
            DeleteFileSystemInfo(directory);
        }

        public void EnsureDirectoryExists(DirectoryInfo directory)
        {
            if (directory.Exists)
            {
                return;
            }

            if (directory.Parent != null && !directory.Parent.Exists)
            {
                EnsureDirectoryExists(directory.Parent);
            }

            Directory.CreateDirectory(directory.FullName);            
        }

        public string GetPathForHostEnvironment(string path)
        {
            if (_hostEnvironment.IsRunningOnMono())
            {
                return path.Replace('\\', '/');
            }

            return path.Replace('/', '\\');
        }

        private static void DeleteFileSystemInfo(FileSystemInfo fileSystemInfo)
        {
            var directoryInfo = fileSystemInfo as DirectoryInfo;
            if (directoryInfo != null)
            {
                foreach (var childInfo in directoryInfo.GetFileSystemInfos())
                {
                    DeleteFileSystemInfo(childInfo);
                }
            }

            fileSystemInfo.Attributes = FileAttributes.Normal;
            fileSystemInfo.Delete();
        }

        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);            
        }

        public void RenameDirectory(string path, string name)
        {
            var directoryInfo = new DirectoryInfo(path).Parent;
            if (directoryInfo == null)
            {
                throw new DirectoryNotFoundException(string.Format("Directory not found for path {0}", path));
            }

            Directory.Move(path, Path.Combine(directoryInfo.FullName, name));
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