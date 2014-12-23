using Crane.Core.Configuration;

namespace Crane.Core.Templates.Parsers
{
    public interface ITokenDictionaryFactory
    {
        ITokenDictionary Create(ICraneContext craneContext, IProjectContext projectContext);
    }
}