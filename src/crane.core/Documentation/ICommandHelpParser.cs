using System.Collections.Generic;

namespace Crane.Core.Documentation
{
    public interface ICommandHelpParser
    {
        ICommandHelpCollection Parse(string documentation);
    }
}