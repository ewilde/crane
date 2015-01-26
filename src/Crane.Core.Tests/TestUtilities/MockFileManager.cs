using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Crane.Core.IO;
using FakeItEasy;

namespace Crane.Core.Tests.TestUtilities
{
    public class MockFileManager : IFileManager
    {
        private readonly StringBuilder _stringBuilder;
        private readonly IFileManager _fake;
        public MockFileManager()
        {
            _stringBuilder = new StringBuilder();
            _fake = A.Fake<IFileManager>();
        }

        public string Output
        {
            get { return _stringBuilder.ToString(); }
        }

        public IFileManager UnderlyingFake
        {
            get { return _fake; }
        }

        public void CopyFiles(string source, string destination, bool copySubDirectories)
        {
            _fake.CopyFiles(source, destination, copySubDirectories);
        }

        public string CurrentDirectory
        {
            get { return _fake.CurrentDirectory; }
        }

        public bool FileExists(string path)
        {
            return _fake.FileExists(path);
        }

        public void CreateDirectory(string destination)
        {
            _fake.CreateDirectory(destination);
        }

        public bool DirectoryExists(string directory)
        {
            return _fake.DirectoryExists(directory);
        }

        public IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return _fake.EnumerateFiles(path, searchPattern, searchOption);
        }

        public IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            return _fake.EnumerateDirectories(path, searchPattern, searchOption);
        }

        public string ReadAllText(string path)
        {
            return _fake.ReadAllText(path);
        }

        public void RenameDirectory(string path, string name)
        {
            _fake.RenameDirectory(path, name);
        }

        public void WriteAllText(string path, string text)
        {
            _fake.WriteAllText(path, text);
            _stringBuilder.Append(text);
        }

        public string GetTemporaryDirectory()
        {
            return _fake.GetTemporaryDirectory();
        }

        public void RenameFile(string path, string name)
        {
            _fake.RenameFile(path, name);
        }

        public void Delete(DirectoryInfo directory)
        {
            _fake.Delete(directory);
        }

        public void EnsureDirectoryExists(DirectoryInfo directory)
        {
            _fake.EnsureDirectoryExists(directory);
        }

        public string GetPathForHostEnvironment(string path)
        {
            return _fake.GetPathForHostEnvironment(path);
        }
    }
}