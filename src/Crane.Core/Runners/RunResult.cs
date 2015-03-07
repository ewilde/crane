namespace Crane.Core.Runners
{
    public class RunResult
    {
        public RunResult(string commandExecuted, string standardOutput, string errorOutput, int exitCode)
        {
            StandardOutput = standardOutput;
            ErrorOutput = errorOutput;
            ExitCode = exitCode;
            CommandExecuted = commandExecuted;
        }

        public string StandardOutput { get; private set; }

        public string ErrorOutput { get; private set; }

        public int ExitCode { get; private set; }

        public string CommandExecuted { get; private set; }

        public override string ToString()
        {
            return string.Format("Command: {0}, StandardOutput: {1}, ErrorOutput: {2}, ExitCode: {3}", CommandExecuted,
                StandardOutput, ErrorOutput,
                ExitCode);
        }
    }
}