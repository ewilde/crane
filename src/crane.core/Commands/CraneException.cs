using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crane.Core.Commands
{
    [Serializable]
    public class CraneException : Exception
    {
      public CraneException() { }
      public CraneException( string message ) : base( message ) { }
      public CraneException( string message, Exception inner ) : base( message, inner ) { }
      protected CraneException( 
	    System.Runtime.Serialization.SerializationInfo info, 
	    System.Runtime.Serialization.StreamingContext context ) : base( info, context ) { }
    }
}
