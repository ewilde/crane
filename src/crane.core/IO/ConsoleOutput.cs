using System;

namespace Crane.Core.IO
{
    public class ConsoleOutput : IOutput
    {
        public void WriteSuccess(string message, params object [] args)
        {
            PrintColor(message, ConsoleColor.DarkGreen, ConsoleColor.Black, args);
        }

        public void WriteError(string message, params object[] args)
        {
            PrintColor(message, ConsoleColor.Red, ConsoleColor.Black, args);
        }

        public void WriteDebug(string message, params object[] args)
        {
            PrintColor(message, ConsoleColor.Gray, ConsoleColor.Black, args);
        }

        public void WriteWarning(string message, params object[] args)
        {
            PrintColor(message, ConsoleColor.Yellow, ConsoleColor.Black, args);
        }

        public void WriteInfo(string message, params object[] args)
        {
            PrintColor(message, ConsoleColor.White, ConsoleColor.Black, args);
        }

        public void WriteFatal(string message, params object[] args)
        {
            PrintColor(message, ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Black, args);
        }

        public void PrintColor(string message, ConsoleColor color = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black, params object[] args)
        {
            ConsoleColor originalForegroundColor = Console.ForegroundColor;
            ConsoleColor originalBackgroundColor = Console.BackgroundColor;
            try
            {
                Console.ForegroundColor = color;
                Console.BackgroundColor = backgroundColor;

                Console.WriteLine(message, args);
            }
            finally
            {
                Console.ForegroundColor = originalForegroundColor;
                Console.BackgroundColor = originalBackgroundColor;

            }
        }

        public void BlankLine()
        {
            Console.WriteLine(string.Empty);
        }
    }
}