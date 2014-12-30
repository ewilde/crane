using Crane.Core.Templates;

namespace Crane.Core.Commands.Exceptions
{
    public class TemplateNotFoundCraneException : CraneException
    {
        public TemplateNotFoundCraneException(TemplateType templateType)
            : base(string.Format("Template type of {0} not found please check your configuration", templateType.ToString()))
        {
            
        }
    }
}