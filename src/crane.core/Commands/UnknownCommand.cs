using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crane.Core.Commands
{
    public class UnknownCommand : ICraneCommand
    {
        public string Name { get; set; }
    }
}
