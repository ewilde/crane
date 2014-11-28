namespace Crane.Console
{
    public class Program
    {
        static int Main(string[] args)
        {
            var program = new Program(null);
            return program.Run();
        }

        public Program(IOptions options)
        {
            if (!options.Validate())
            {
                options.ShowHelp();
            }
        }

        public int Run()
        {
            return 0;
        }
    }
}
