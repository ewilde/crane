namespace Crane.Core.Configuration
{
    public class DefaultConfiguration : IConfiguration
    {
        public string BuildTemplateProviderName
        {
            get { return "Psake"; } 
        }
    }
}