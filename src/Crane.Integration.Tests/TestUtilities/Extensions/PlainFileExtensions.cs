using Crane.Core.Api.Builders;

namespace Crane.Integration.Tests.TestUtilities.Extensions
{
    public static class PlainFileExtensions
    {
        public static void AddSolutionPackagesConfigWithXUnitRunner(this PlainFile text, string path)
        {
            text.Path = path;
            text.Text = @"<?xml version=""1.0"" encoding=""utf-8""?>
<packages>
  <package id=""xunit.runners"" version=""1.9.2"" />
</packages>";
        }
    }
}