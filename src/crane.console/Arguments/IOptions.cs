namespace Crane.Console
{
    public interface IOptions
    {
        string[] Arguments { get; }

        void ShowHelp();

        bool Validate();
    }
}