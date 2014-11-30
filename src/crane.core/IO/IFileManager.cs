using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Crane.Core.IO
{
    public interface IFileManager
    {
        IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption);

        void CopyFiles(string source, string destination, string filter);
        
        bool DirectoryExists(string directory);

        void CreateDirectory(string destination);

        string ReadAllText(string path);

        void WriteAllText(string path, string text);
    }
}