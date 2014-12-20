using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Crane.Core.Documentation
{
    public class CommandHelpParser : ICommandHelpParser
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
                var examples = new List<CommandExample>();
                commands.Add(fullName, new CommandHelp(name, GetValueOfDefault(member, "summary", string.Empty), examples));    
            }

            return new CommandHelpCollection(commands);
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