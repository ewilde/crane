using System.IO;

namespace Crane.Core.Templates.Parsers
{
    public interface ITemplateParser
    {
        string Parse(string template, object model);
    }
}
