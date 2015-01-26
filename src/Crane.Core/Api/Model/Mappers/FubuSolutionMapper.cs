using System;
using System.Linq;
using Crane.Core.Api.Mappers;

namespace Crane.Core.Api.Model.Mappers
{
    [CLSCompliant(false)]
    public class FubuSolutionMapper : IFubuSolutionMapper
    {
        private readonly IFubuProjectMapper _mapper;

        public FubuSolutionMapper(IFubuProjectMapper mapper)
        {
            _mapper = mapper;
        }

        public Solution Map(FubuCsProjFile.Solution solution)
        {
            var result = new Solution
            {
                Name = solution.Name, 
                Path = solution.Filename
            };

            result.Projects = solution.Projects.Select(item =>
            {
                var project = _mapper.Map(item.Project);
                project.Solution = result;
                return project;
            });

            return result;
        }
    }
}