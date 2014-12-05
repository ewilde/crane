using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crane.Core.IO;
using Crane.Core.Utility;

namespace Crane.Integration.Tests.TestUtilities
{
    public class CraneTestContext
    {
        private readonly IFileManager _fileManager;
        private DirectoryInfo _directory;

        public string Directory
        {
            get { return _directory.FullName; }
        }

        public CraneTestContext(IFileManager fileManager)
        {
            _fileManager = fileManager;
            _directory = new DirectoryInfo(_fileManager.GetTemporaryDirectory());
            var currentDir = AssemblyUtility.GetLocation(typeof (CraneTestContext).Assembly);
            _fileManager.CopyFiles(currentDir.FullName, _directory.FullName, true);
        }

        public void TearDown()
        {
            _fileManager.Delete(_directory);
        }
    }
}
