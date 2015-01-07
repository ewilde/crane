﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crane.Core.Templates.Parsers
{
    public interface IFileAndDirectoryTokenParser
    {
        void Parse(DirectoryInfo path);
    }
}