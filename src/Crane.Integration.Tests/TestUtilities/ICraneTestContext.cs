namespace Crane.Integration.Tests.TestUtilities
{
    public interface ICraneTestContext
    {
        /// <summary>
        /// This directory contains the crane.exe
        /// </summary>
        string BuildOutputDirectory { get; }

        /// <summary>
        /// Root test context directory. Sub-folders will be 'doc', 'build-output' etc...
        /// </summary>
        string RootDirectory { get; }
    }
}