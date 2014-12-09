using System;
using System.Collections;
using System.Collections.Generic;

namespace Crane.Core.Commands.Exceptions
{
    [Serializable]
    public abstract class CraneException : Exception
    {
        protected CraneException() { }
        protected CraneException( string message ) : base( message ) { }
        protected CraneException( string message, Exception inner ) : base( message, inner ) { }
        protected CraneException( 
	        System.Runtime.Serialization.SerializationInfo info, 
	        System.Runtime.Serialization.StreamingContext context ) : base( info, context ) { }
    }

    public class MissingArgumentCraneException : CraneException
    {
        public IEnumerable<string> MissingArguments { get; private set; }

        public MissingArgumentCraneException(IEnumerable<string> missingArguments)
            : base("Arguments missing")
        {
            
        }
    }

    public class DirectoryExistsCraneException : CraneException
    {
        public DirectoryExistsCraneException(string directoryName)
            : base(string.Format("directory {0} already exists", directoryName))
        {
            
        }
    }
}
