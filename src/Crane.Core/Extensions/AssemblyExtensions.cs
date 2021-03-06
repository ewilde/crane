﻿using System;
using System.IO;
using System.Reflection;

namespace Crane.Core.Extensions
{
    public static class AssemblyExtensions
    {
        public static DirectoryInfo GetLocation(this Assembly assembly)
        {
            string codeBase = assembly.CodeBase;
            var uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return new DirectoryInfo(Path.GetDirectoryName(path));
        }
    }
}