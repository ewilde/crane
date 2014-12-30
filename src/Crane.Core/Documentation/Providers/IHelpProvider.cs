namespace Crane.Core.Documentation.Providers
{
    public interface IHelpProvider
    {
        ICommandHelpCollection HelpCollection { get; } 
    }
}