using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace Crane.Core.Documentation.Parsers
{
    public class XmlCommentCommandHelpParser : ICommandHelpParser
    {
        public ICommandHelpCollection Parse(string documentation)
        {
            var document = XDocument.Parse(documentation);
            var commands = new Dictionary<string, ICommandHelp>();

            foreach (var member in document.Descendants("member")
                .Where(item => 
                    item.Attribute("name") != null && 
                    item.Attribute("name").Value.StartsWith(@"T:Crane.Core.Commands")))
            {
                var fullName = member.Attribute("name").Value.TrimStart(new[] {'T', ':'});
                var name = fullName.Split('.').Last().ToLower();
                var examples = GetExamples(member);
                commands.Add(name, new CommandHelp(name, fullName, GetValueOfDefault(member, "summary", string.Empty).Trim(), examples));    
            }

            return new CommandHelpCollection(commands);
        }

        private static List<CommandExample> GetExamples(XElement memberElement)
        {
            var examples = new List<CommandExample>();
            foreach (var example in memberElement.Elements("example"))
            {
                examples.Add(new CommandExample { Value = example.Value.Trim()});
            }

            return examples;
        }

        private static string GetValueOfDefault(XElement member, string name, string defaultValue)
        {
            if (member.Element(name) != null)
            {
                return member.Element(name).Value;
            }

            return defaultValue;
        }
    }
}