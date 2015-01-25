using Crane.Core.Api.Model;

namespace Crane.Core.Api.Builders
{
    public class ProjectBuilder
    {
        private Project _project;

        public ProjectBuilder()
        {
            _project = new Project();
        }


        public ProjectBuilder WithName(string name)
        {
            _project.Name = name;
            return this;
        }

        public Project Build()
        {
            return null;
        }
    }
}