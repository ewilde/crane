using System.IO;

namespace Crane.Core.Templates.Resolvers
{
    public interface ITemplateFactory
    {
        ITemplate Create(DirectoryInfo directoryInfo, TemplateType templateType);
    }
}