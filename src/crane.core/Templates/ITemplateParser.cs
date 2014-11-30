using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crane.Core.Templates
{
    public interface ITemplateParser
    {
        string Parse(FileInfo template, object model);
    }
}
