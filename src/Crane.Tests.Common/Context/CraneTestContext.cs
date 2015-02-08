using System;
using System.IO;
using Crane.Core.Extensions;
using Crane.Core.IO;
using Crane.Tests.Common.Runners;
using log4net;

namespace Crane.Tests.Common.Context
{
    public class CraneTestContext : ICraneTestContext
    {
        private readonly IFileManager _fileManager;
        private readonly DirectoryInfo _rootDirectory;
        private readonly DirectoryInfo _gitRepoRootDirectory;
        private static readonly ILog _log = LogManager.GetLogger(typeof(CraneTestContext));

        /// <summary>
        /// This directory contains the crane.exe
        /// </summary>
        public string BuildOutputDirectory
        {
            get { return Path.Combine(RootDirectory, "build-output"); }
        }

        /// <summary>
        /// Root test context directory. Sub-folders will be 'doc', 'build-output' etc...
        /// </summary>
        public string RootDirectory
        {
            get { return _rootDirectory.FullName; }
        }

        public string ToolsDirectory
        {
            get { return Path.Combine(RootDirectory, "tools"); }
        }

        private string CraneGitRepo
        {
            get { return _gitRepoRootDirectory.FullName; }
        }

        public CraneTestContext(IFileManager fileManager)
        {
            _fileManager = fileManager;
            _rootDirectory = new DirectoryInfo(_fileManager.GetTemporaryDirectory());
            
            Directory.CreateDirectory(BuildOutputDirectory);
            var sourceDir = typeof (CraneTestContext).Assembly.GetLocation();
            _gitRepoRootDirectory = GetGitRepoRootDirectory(sourceDir);
            _fileManager.CopyFiles(sourceDir.FullName, BuildOutputDirectory, true);
            _log.DebugFormat("Copied integration test files from {0} to {1}", sourceDir.FullName, BuildOutputDirectory);

            File.Copy(Path.Combine(CraneGitRepo, "mkdocs.yml"), Path.Combine(RootDirectory, "mkdocs.yml"));
            _log.DebugFormat("Copied other mkdoc.yml from {0} to {1}", Path.Combine(_gitRepoRootDirectory.FullName, "mkdocs.yml"), Path.Combine(_rootDirectory.FullName, "mkdocs.yml"));

            CopyTools();
        }

        private void CopyTools()
        {
            _fileManager.CopyFiles(Path.Combine(_gitRepoRootDirectory.FullName, "tools"), ToolsDirectory, true);
        }

        private static DirectoryInfo GetGitRepoRootDirectory(DirectoryInfo currentDir)
        {
            if (Directory.Exists(Path.GetFullPath(Path.Combine(currentDir.FullName, @"..", "doc"))))
            {
                return new DirectoryInfo(Path.GetFullPath(Path.Combine(currentDir.FullName, @"..")));
            }

            return new DirectoryInfo(Path.GetFullPath(Path.Combine(currentDir.FullName, string.Format("..{0}..{0}..{0}..{0}", Path.DirectorySeparatorChar))));
        }

        public void TearDown()
        {
            try
            {
                _fileManager.Delete(_rootDirectory);
            }
            catch (Exception exception)
            {
                _log.Warn(string.Format("Error tearing down test, trying to delete temp directory {0}.", RootDirectory), exception);
            }            
        }
    }
}
