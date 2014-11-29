using System.Collections.Generic;

namespace Crane.Core.Templates.Resolvers
{
    public interface ITemplateResolver
    {
        IEnumerable<ITemplate> Templates { get; set; }
    }
}