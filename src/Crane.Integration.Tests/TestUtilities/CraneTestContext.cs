using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crane.Core.Extensions;
using Crane.Core.IO;
using log4net;

namespace Crane.Integration.Tests.TestUtilities
{
    public class CraneTestContext
    {
        private readonly IFileManager _fileManager;
        private readonly DirectoryInfo _directory;
        private readonly DirectoryInfo _rootDirectory;
        private readonly DirectoryInfo _gitRepoRootDirectory;
        private static readonly ILog _log = LogManager.GetLogger(typeof(CraneTestContext));

        /// <summary>
        /// This directory contains the crane.exe
        /// </summary>
        public string BuildOutputDirectory
        {
            get { return _directory.FullName; }
        }

        /// <summary>
        /// Root test context directory. Sub-folders will be 'doc', 'build-output' etc...
        /// </summary>
        public string RootDirectory
        {
            get { return _rootDirectory.FullName; }
        }

        public CraneTestContext(IFileManager fileManager)
        {
            _fileManager = fileManager;
            _rootDirectory = new DirectoryInfo(_fileManager.GetTemporaryDirectory());
            _directory = System.IO.Directory.CreateDirectory(Path.Combine(_rootDirectory.FullName, "build-output"));
            var currentDir = typeof (CraneTestContext).Assembly.GetLocation();
            _gitRepoRootDirectory = GetGitRepoRootDirectory(currentDir);
            _fileManager.CopyFiles(currentDir.FullName, _directory.FullName, true);
            _log.DebugFormat("Copied integration test files from {0} to {1}", currentDir.FullName, _directory.FullName);

            File.Copy(Path.Combine(_gitRepoRootDirectory.FullName, "mkdocs.yml"), Path.Combine(_rootDirectory.FullName, "mkdocs.yml"));
            _log.DebugFormat("Copied other mkdoc.yml from {0} to {1}", Path.Combine(_gitRepoRootDirectory.FullName, "mkdocs.yml"), Path.Combine(_rootDirectory.FullName, "mkdocs.yml"));
        }

        private static DirectoryInfo GetGitRepoRootDirectory(DirectoryInfo currentDir)
        {
            if (Directory.Exists(Path.GetFullPath(Path.Combine(currentDir.FullName, @"..", "doc"))))
            {
                return new DirectoryInfo(Path.GetFullPath(Path.Combine(currentDir.FullName, @"..")));
            }

            return new DirectoryInfo(Path.GetFullPath(Path.Combine(currentDir.FullName, @"..\..\..\..\")));
        }

        public void TearDown()
        {
            try
            {
                _fileManager.Delete(_directory);
            }
            catch (Exception exception)
            {
                _log.Warn(string.Format("Error tearing down test, trying to delete temp directory {0}.", _directory.FullName), exception);
            }            
        }
    }
}
