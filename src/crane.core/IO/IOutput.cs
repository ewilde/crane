namespace Crane.Core.IO
{
    public interface IOutput
    {
        void WriteLine(string format, params object [] args);
    }
}