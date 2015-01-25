using System;
using System.IO;
using System.Linq;
using Crane.Core.Api.Mappers;
using Crane.Core.Api.Model;
using Crane.Core.Api.Model.Mappers;

namespace Crane.Core.Api.Readers
{
    [CLSCompliant(false)]
    public class FubuSolutionReader : ISolutionReader
    {
        private readonly IFubuSolutionMapper _mapper;

        public FubuSolutionReader(IFubuSolutionMapper mapper)
        {
            _mapper = mapper;
        }

        public Solution FromPath(string path)
        {
            var solution = _mapper.Map(FubuCsProjFile.Solution.LoadFrom(path));
            return solution;
        }
    }
}