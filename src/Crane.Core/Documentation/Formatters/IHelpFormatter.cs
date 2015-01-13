namespace Crane.Core.Documentation.Formatters
{
    public interface IHelpFormatter
    {
        string Format(ICommandHelp commandHelp);

        string FormatSummary(ICommandHelp commandHelp);
    }
}