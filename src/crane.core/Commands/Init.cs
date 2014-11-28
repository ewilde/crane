namespace Crane.Core.Commands
{
    public class Init : ICraneCommand
    {
        public string NoMatchingMethodsText()
        {
            return "Did you mean crane init projectname";
        }

        public void Execute(string projectName)
        {
            
        }
    }
}