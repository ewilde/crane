using System;
using System.Text;
using Crane.Core.IO;

namespace Crane.Core.Tests.TestUtilities
{
    public class MockOutput : IOutput
    {
        private StringBuilder result;

        public MockOutput()
        {
            result = new StringBuilder();
        }

        public void WriteInfo(string format, params object[] args)
        {
            result.AppendLine(string.Format(format, args));
        }

        public void WriteSuccess(string format, params object[] args)
        {
            result.AppendLine(string.Format(format, args));
        }

        public void WriteError(string format, params object[] args)
        {
            result.AppendLine(string.Format(format, args));
        }

        public void WriteDebug(string format, params object[] args)
        {
            result.AppendLine(string.Format(format, args));
        }

        public void WriteWarning(string format, params object[] args)
        {
            result.AppendLine(string.Format(format, args));
        }

        public override string ToString()
        {
            return result.ToString();
        }
    }
}