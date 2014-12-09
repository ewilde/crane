namespace Crane.Core.IO
{
    public interface IOutput
    {
        void WriteInfo(string format, params object[] args);
        void WriteSuccess(string format, params object[] args);
        void WriteError(string format, params object[] args);
        void WriteDebug(string format, params object[] args);
        void WriteWarning(string format, params object[] args);

    }

}