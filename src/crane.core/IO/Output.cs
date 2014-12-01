using System;

namespace Crane.Core.IO
{
    public class Output : IOutput
    {
        public void WriteLine()
        {
            Console.WriteLine();
        }

        public void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }
    }
}