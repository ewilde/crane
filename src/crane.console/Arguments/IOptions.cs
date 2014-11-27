namespace crane.console
{
    public interface IOptions
    {
        string[] Arguments { get; }

        void ShowHelp();

        bool Validate();
    }
}