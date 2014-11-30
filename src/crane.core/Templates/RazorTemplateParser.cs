using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crane.Core.IO;
using log4net;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace Crane.Core.Templates
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

        public string Parse(FileInfo template, object model)
        {
            try
            {
                return Razor.Parse(_fileManager.ReadAllText(template.FullName), model);
            }
            catch (Exception exception)
            {
                _log.Error(string.Format("Problem parsing template {0}", template.FullName), exception);
                throw;
            }
        }
    }
}
