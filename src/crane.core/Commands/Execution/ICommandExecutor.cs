using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crane.Core.Commands.Execution
{
    public interface ICommandExecutor
    {
        int ExecuteCommand(string[] arguments);
    }
}
