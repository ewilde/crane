using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crane.Core.Commands.Parsers;
using Crane.Core.Extensions;

namespace Crane.Core.Documentation.Formatters
{
    public class MarkdownHelpFormatter : IHelpFormatter
    {
        private readonly ICommandTypeInfoParser _typeInfoParser;

        public MarkdownHelpFormatter(ICommandTypeInfoParser typeInfoParser)
        {
            _typeInfoParser = typeInfoParser;
        }

        public string Format(ICommandHelp commandHelp)
        {
            var result = new StringBuilder();
            result.AppendLine(FormatUsage(commandHelp));

            result.AppendLine();
            result.AppendLine(FormatDescription(commandHelp));

            if (commandHelp.Examples.Any())
            {
                result.Append(FormatExamples(commandHelp.Examples));
                result.AppendLine();
            }

            return result.ToString();
        }

        public string FormatSummary(ICommandHelp commandHelp)
        {
            var result = new StringBuilder();
            AddLines(result, commandHelp.Description);
            return string.Format("* [`crane {0}`]({0}.md)  {2}{1}{2}", commandHelp.CommandName, result, Environment.NewLine);
        }

        private string FormatDescription(ICommandHelp commandHelp)
        {
            var result = new StringBuilder();
            AddLines(result, commandHelp.Description);

            return result.ToString();
        }

        private string FormatExamples(IEnumerable<CommandExample> examples)
        {
            var result = new StringBuilder();
            examples.ForEach(
                item => AddExampleLines(result, item.Value));

            return result.ToString();
        }

        private void AddExampleLines(StringBuilder result, string value)
        {
            var lines = value.Lines();
            if (lines.Length == 0)
            {
                return;
            }

            result.AppendLine(string.Format("**{0}**  ", lines[0]));
            AddLines(result, lines.Skip(1));
        }

        private void AddLines(StringBuilder result, string value)
        {
            var lines = value.Lines();
            if (lines.Length == 0)
            {
                return;
            }

            AddLines(result, lines);
        }

        private void AddLines(StringBuilder result, IEnumerable<string> lines)
        {
            var items = lines as string[] ?? lines.ToArray();
            int removePaddingCount = GetRemovePaddingCount(items.ToArray());

            items.ForEach(
                line => GetMarkDownLine(result, line, removePaddingCount));
        }

        private void GetMarkDownLine(StringBuilder result, string line, int removePaddingCount)
        {
            line = line.Trim(' ', removePaddingCount);
            if (line.Contains("<code>") && line.Contains("</code>"))
            {
                result.AppendLine(line.Replace("<code>", "`").Replace("</code>", "`"));
            }
            else
            {
                result.AppendLine(line.Replace("<code>", "```").Replace("</code>", "```"));
            }
        }

        private int GetRemovePaddingCount(string[] lines)
        {
            return lines.Length > 1 ? lines[1].PadCountLeft() : 0;
        }

        private string FormatUsage(ICommandHelp commandHelp)
        {
            var requiredArgs = new StringBuilder();
            _typeInfoParser.GetArguments(commandHelp.CommandType)
                .Where(item => item.Required)
                .ForEach(item => requiredArgs.AppendFormat("<{0}>", item.Name.PascalCaseToWords().ToLower()));

            return string.Format("`usage: crane {0} {1}`", commandHelp.CommandName, requiredArgs);
        }
    }
}
