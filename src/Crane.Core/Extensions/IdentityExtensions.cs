using System.Security.Principal;

namespace Crane.Core.Extensions
{
    public static class IdentityExtensions
    {
        public static bool IsElevated(this WindowsIdentity value)
        {
            var principal = new WindowsPrincipal(value);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}