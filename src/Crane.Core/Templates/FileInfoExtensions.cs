using System.IO;

namespace Crane.Core.Templates
{
    public static class FileInfoExtensions
    {
        public static bool IsTextFile(this FileInfo fileInfo)
        {
            return !fileInfo.Extension.Equals(".exe");
        }
    }
}