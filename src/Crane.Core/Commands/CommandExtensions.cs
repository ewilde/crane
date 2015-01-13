namespace Crane.Core.Commands
{
    public static class CommandExtensions
    {
        public static string Name(this ICraneCommand value)
        {
            return value.GetType().Name.ToLowerInvariant();
        }
    }
}