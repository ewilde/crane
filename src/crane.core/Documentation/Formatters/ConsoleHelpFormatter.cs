using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crane.Core.Commands.Parsers;
using Crane.Core.Utility;
using PowerArgs;

namespace Crane.Core.Documentation.Formatters
{
    /// <summary>
    /// Console help formatter uses the following syntax
    /// 
    /// usage: crane {command name} &lt;default argument&gt; [Required named argument 1...] [Required named argument 2...] [options]
    /// 
    /// {command description}
    /// 
    /// options:
    /// -NamedArgument1         Named argument 1 description
    /// -NamedArgument2         Named argument 2 description
    /// 
    /// example 1:
    /// {content}
    /// 
    /// example 2:
    /// {content}
    /// 
    /// For more information, visit {command reference link}
    /// </summary>
    /// <remarks>
    /// If there are no arguments other than the default argument [option] is not displayed in the usage statement and the options list is not displayed.
    /// </remarks>
    public class ConsoleHelpFormatter : IHelpFormatter
    {
        private readonly ICommandTypeInfoParser _typeInfoParser;

        public ConsoleHelpFormatter(ICommandTypeInfoParser typeInfoParser)
        {
            _typeInfoParser = typeInfoParser;
        }

        public string Format(ICommandHelp commandHelp)
        {
            var result = new StringBuilder();
            result.AppendLine(FormatUsage(commandHelp));
            result.AppendLine();
            result.AppendLine(FormatDescription(commandHelp));
            result.AppendLine();

            if (commandHelp.Examples.Any())
            {
                result.Append(FormatExamples(commandHelp.Examples));
                result.AppendLine();
            }

            result.AppendLine(MoreInformation());
            return result.ToString();
        }

        private string MoreInformation()
        {
            return "For more information, visit https://github.com/ewilde/crane"; // update with commandline reference docs once we have some
        }

        private string FormatExamples(IEnumerable<CommandExample> examples)
        {
            var result = new StringBuilder();
            examples.ForEach(
                item =>
                {
                    var lines = item.Value.Split(new string[] {Environment.NewLine, "\n"}, StringSplitOptions.None);
                    if (lines.Length == 0)
                    {
                        return;    
                    }

                    int removePaddingCount = lines.Length > 1 ? lines[1].PadCountLeft() : 0;

                    lines.ForEach(
                        line =>
                            result.AppendLine(
                                line.Trim(' ', removePaddingCount)
                                    .Replace("<code>", string.Empty)
                                    .Replace("</code>", string.Empty)));                    
                });

            return result.ToString();
        }

        private string FormatDescription(ICommandHelp commandHelp)
        {
            return commandHelp.Description;
        }

        private string FormatUsage(ICommandHelp commandHelp)
        {
            var requiredArgs = new StringBuilder();
            _typeInfoParser.GetArguments(commandHelp.CommandType)
                .Where(item => item.Required)
                .ForEach(item => requiredArgs.AppendFormat("<{0}>", item.Name.PascalCaseToWords().ToLower()));

            return string.Format("usage: crane {0} {1}", commandHelp.CommandName, requiredArgs);
        }       
    }
}