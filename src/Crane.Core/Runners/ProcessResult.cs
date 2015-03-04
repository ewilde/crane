namespace Crane.Core.Runners
{
    public class ProcessResult
    {
        public string StandardOutput { get; set; }
        public string ErrorOutput { get; set; }
        public int ExitCode { get; set; }
    }
}