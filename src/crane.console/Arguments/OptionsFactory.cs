namespace crane.console
{
    public class OptionsFactory
    {
        IOptions Create(string[] arguments)
        {
            if (arguments == null || arguments.Length == 0)
            {
                return new EmptyOptions(arguments);
            }

            return null;
        }
    }
}