using System;
using System.Diagnostics;

namespace Crane.Core.Api.Model
{
    public class AssemblyInfo : ProjectFile
    {
        public AssemblyInfo()
        {
            this.Version = new Version();
            this.FileVersion = new Version();
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public Version Version { get; set; }

        public Version FileVersion { get; set; }

        /// <summary>
        /// Appears under <see cref="FileVersionInfo.ProductVersion"/>
        /// </summary>
        public string InformationalVersion { get; set; }
    }
}