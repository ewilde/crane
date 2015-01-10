using System.IO;
using System.Reflection;
using Crane.Core.Documentation.Parsers;
using Crane.Core.Extensions;
using Crane.Core.IO;

namespace Crane.Core.Documentation.Providers
{
    public class XmlHelpProvider : IHelpProvider
    {
        private readonly ICommandHelpCollection _helpCollection;

        public XmlHelpProvider(ICommandHelpParser parser, IFileManager fileManager)
        {

            _helpCollection =
                parser.Parse(
                    fileManager.ReadAllText(GetPath()));
        }

        private string GetPath()
        {
            var location = Assembly.GetExecutingAssembly().GetLocation().FullName;
            if (File.Exists(Path.Combine(location, "Crane.Core.XML"))) // msbuild & xbuild create the documentation file with upper case XML
                return Path.Combine(location, "Crane.Core.XML");
            
            if (File.Exists(Path.Combine(location, "Crane.Core.xml"))) // ends up copied to dependant directories with lower case for some reason
                return Path.Combine(location, "Crane.Core.xml");

            throw new FileNotFoundException(string.Format("Could not find file Crane.Core.XML or Crane.Core.xml in directory{0}", location));
        }

        public ICommandHelpCollection HelpCollection
        {
            get { return _helpCollection; }
        }
    }
}