using System;
using System.Runtime.Serialization;

namespace Crane.Core.Api.Exceptions
{
    [Serializable]
    public class NugetException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public NugetException()
        {
        }

        public NugetException(string message) : base(message)
        {
        }

        public NugetException(string message, Exception inner) : base(message, inner)
        {
        }

        protected NugetException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }    
}