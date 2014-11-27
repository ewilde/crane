using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crane.console
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
