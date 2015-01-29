using System;

namespace Crane.Tests.Common
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
