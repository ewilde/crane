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
            //if (!options.Validate())
            //{
            //    options.Help();
            //}
        }

        public int Run()
        {
            System.Console.WriteLine("Did you mean blah blah");
            return 0;
        }
    }
}
