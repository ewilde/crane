using System.Text;
using System.Xml.Linq;

namespace Crane.Core.Extensions
{
    public static class XDocumentExtensions
    {
        public static string InnerXml(this XElement element)
        {
            var innerXml = new StringBuilder();
            foreach (XNode node in element.Nodes())
            {
                innerXml.Append(node);
            }

            return innerXml.ToString();
        } 
    }
}