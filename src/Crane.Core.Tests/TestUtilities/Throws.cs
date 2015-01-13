using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crane.Integration.Tests.TestUtilities
{
    public static class Throws
    {
        public static Exception Exception(Action action)
        {
            System.Exception exception = null;
            try
            {
                action();
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            return exception;
        }
    }
}
