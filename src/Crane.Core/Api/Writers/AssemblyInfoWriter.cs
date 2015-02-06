using System;
using System.IO;
using System.Text;
using Crane.Core.Api.Model;
using Crane.Core.Extensions;
using Crane.Core.IO;
using Crane.Core.Properties;

namespace Crane.Core.Api.Writers
{
    public class AssemblyInfoWriter : IAssemblyInfoWriter
    {
        private readonly IFileManager _fileManager;

        public AssemblyInfoWriter(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }

        public void Patch(AssemblyInfo assemblyInfo)
        {
            var lines = this._fileManager.ReadAllText(assemblyInfo.Path).Lines();
            this.UpdateLine(lines, "AssemblyTitle", assemblyInfo.Title);
            this.UpdateLine(lines, "AssemblyDescription", assemblyInfo.Description);

            if (assemblyInfo.Version != null)
                this.UpdateLine(lines, "AssemblyVersion", assemblyInfo.Version.ToString());

            if (assemblyInfo.FileVersion != null)
                this.UpdateLine(lines, "AssemblyFileVersion", assemblyInfo.FileVersion.ToString());

            this.UpdateLine(lines, "AssemblyInformationalVersion", assemblyInfo.InformationalVersion);

            var result = new StringBuilder();
            Array.ForEach(lines, s => result.AppendLine(s));
            this._fileManager.WriteAllText(assemblyInfo.Path, result.ToString().TrimEnd(Environment.NewLine.ToCharArray()));
        }

        public void Create(string path, string title, string description, string version, string informationalVersion, string fileVersion)
        {
            if (_fileManager.FileExists(path))
            {
                throw new InvalidOperationException(string.Format("An assemblyinfo file already exists at {0}.", path));    
            }

            var template = Resources.AssemblyInfoTemplate;
            template = template.Replace("%title%", title);
            template = template.Replace("%description%", description);
            template = template.Replace("%version%", version);
            template = template.Replace("%informationalVersion%", informationalVersion);
            template = template.Replace("%fileVersion%", fileVersion);
            _fileManager.EnsureDirectoryExists(new FileInfo(path).Directory);
            _fileManager.WriteAllText(path, template);
        }

        private void UpdateLine(string[] lines, string property, string value)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                if (Match(property, lines[i]))
                {
                    lines[i] = UpdateValueBetweenQuotes(lines[i], value);
                    break;
                }
            }
        }

        private static bool Match(string property, string line)
        {
            if (line.Trim().StartsWith("//"))
            {
                return false;
            }

            return line.IndexOf(property, StringComparison.InvariantCultureIgnoreCase) > -1;
        }

        private string UpdateValueBetweenQuotes(string line, string value)
        {
            var start = line.IndexOf('"') + 1;
            var end = line.IndexOf('"', start);

            return line.Substring(0, start) + value + line.Substring(end);
        }
    }
}