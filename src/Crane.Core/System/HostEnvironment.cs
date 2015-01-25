using System;

namespace Crane.Core
{
    public class HostEnvironment : IHostEnvironment
    {
        public bool IsRunningOnMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }
    }
}