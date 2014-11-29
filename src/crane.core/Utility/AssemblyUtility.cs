using System;
using System.IO;
using System.Reflection;

namespace Crane.Core.Utility
{
    public static class AssemblyUtility
    {
        public static DirectoryInfo GetLocation(Assembly assembly)
        {
            string codeBase = assembly.CodeBase;
            var uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return new DirectoryInfo(Path.GetDirectoryName(path));
        }
    }
}