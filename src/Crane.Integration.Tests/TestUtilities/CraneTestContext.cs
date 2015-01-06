using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crane.Core.IO;
using Crane.Core.Utility;
using log4net;

namespace Crane.Integration.Tests.TestUtilities
{
    public class CraneTestContext
    {
        private readonly IFileManager _fileManager;
        private readonly DirectoryInfo _directory;
        private static readonly ILog _log = LogManager.GetLogger(typeof(CraneTestContext));

        public string Directory
        {
            get { return _directory.FullName; }
        }

        public CraneTestContext(IFileManager fileManager)
        {
            _fileManager = fileManager;
            _directory = new DirectoryInfo(_fileManager.GetTemporaryDirectory());
            var currentDir = typeof (CraneTestContext).Assembly.GetLocation();
            _fileManager.CopyFiles(currentDir.FullName, _directory.FullName, true);
            _log.DebugFormat("Copied files from {0} to {1}", currentDir.FullName, _directory.FullName);
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
