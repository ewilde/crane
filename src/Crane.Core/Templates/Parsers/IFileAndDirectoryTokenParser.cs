using System.IO;

namespace Crane.Core.Templates.Parsers
{
    public interface IFileAndDirectoryTokenParser
    {
        void Parse(DirectoryInfo path, ITokenDictionary tokenDictionary);
    }
}
