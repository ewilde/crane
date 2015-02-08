namespace Crane.Core.Runners
{
    public class RunResult
    {
        public string StandardOutput { get; set; }
        public string ErrorOutput { get; set; }
        public int ExitCode { get; set; }

        public override string ToString()
        {
            return string.Format("StandardOutput: {0}, ErrorOutput: {1}, ExitCode: {2}", StandardOutput, ErrorOutput, ExitCode);
        }
    }
}