using System;
using Crane.Core.IO;
using log4net;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace Crane.Core.Templates.Parsers
{
    public class RazorTemplateParser : ITemplateParser
    {
        private readonly IFileManager _fileManager;
        private static readonly ILog _log = LogManager.GetLogger(typeof(RazorTemplateParser));

        public RazorTemplateParser(IFileManager fileManager)
        {
            _fileManager = fileManager;
            var config = new FluentTemplateServiceConfiguration(c => c.WithEncoding(RazorEngine.Encoding.Raw));
            var templateService = new TemplateService(config);
            Razor.SetTemplateService(templateService);
  
        }

        public string Parse(string template, object model)
        {
            try
            {
                return Razor.Parse(template, model);
            }
            catch (Exception exception)
            {
                _log.Error("Problem parsing template", exception);
                throw;
            }
        }
    }
}
