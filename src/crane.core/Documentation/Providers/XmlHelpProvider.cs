using System.IO;
using System.Reflection;
using Crane.Core.Documentation.Parsers;
using Crane.Core.IO;
using Crane.Core.Utility;

namespace Crane.Core.Documentation.Providers
{
    public class XmlHelpProvider : IHelpProvider
    {
        private readonly ICommandHelpCollection _helpCollection;

        public XmlHelpProvider(ICommandHelpParser parser, IFileManager fileManager)
        {

            _helpCollection =
                parser.Parse(
                    fileManager.ReadAllText(Path.Combine(Assembly.GetExecutingAssembly().GetLocation().FullName,
                        "Crane.Core.xml")));
        }

        public ICommandHelpCollection HelpCollection
        {
            get { return _helpCollection; }
        }
    }
}