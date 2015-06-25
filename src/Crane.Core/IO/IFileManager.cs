using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Crane.Core.IO
{
    public interface IFileManager
    {
        void CopyFiles(string source, string destination, bool copySubDirectories);

        string CurrentDirectory { get; }
        bool FileExists(string path);

        void CreateDirectory(string destination);

        bool DirectoryExists(string directory);

        string GetShortPath(string directory);

        IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption);

        IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption);

        string ReadAllText(string path);

        void RenameDirectory(string path, string name);

        void WriteAllText(string path, string text);

        string GetTemporaryDirectory();

        void RenameFile(string path, string name);
        
        void Delete(DirectoryInfo directory);

        void EnsureDirectoryExists(DirectoryInfo directory);

        string GetPathForHostEnvironment(string path);

        string GetFullPath(string path);
    }
}