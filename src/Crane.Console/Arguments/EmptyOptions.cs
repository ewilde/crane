namespace Crane.Console
{
    class EmptyOptions : IOptions
    {
        public EmptyOptions(string[] arguments)
        {
            Arguments = arguments;
        }

        public string[] Arguments { get; private set; }

        public void ShowHelp()
        {
        }

        public bool Validate()
        {
            return false;
        }
    }
}