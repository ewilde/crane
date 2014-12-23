namespace Crane.Core.Templates.Parsers
{
    public interface ITemplateParser
    {
        string Parse(ITokenDictionary tokenDictionary, string template);
    }
}
