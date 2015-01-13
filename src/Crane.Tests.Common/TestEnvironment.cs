using System;

namespace Crane.Integration.Tests
{
    public static class TestEnvironment
    {
        /// <summary>
        /// Determine if runtime is Mono.
        /// Taken from http://stackoverflow.com/questions/721161
        /// </summary>
        /// <returns>True if being executed in Mono, false otherwise.</returns>
        public static bool IsRunningOnMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }
    }
}