using Crane.Core.Configuration;

namespace Crane.Core.Templates
{
    public interface ITemplateInvoker
    {
        void InvokeTemplate(ITemplate template, IProjectContext projectContext);
    }
}