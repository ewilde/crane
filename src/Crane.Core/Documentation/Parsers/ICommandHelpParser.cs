namespace Crane.Core.Documentation.Parsers
{
    public interface ICommandHelpParser
    {
        ICommandHelpCollection Parse(string documentation);
    }
}