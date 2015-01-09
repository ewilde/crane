namespace Crane.Core.Commands.Exceptions
{
    public class MultipleSolutionsFoundCraneException : CraneException
    {
        public MultipleSolutionsFoundCraneException(params string [] solutionNames)
            : base(string.Format("The path should only contain one solution please exlude the others using exclusion rules.  Solutions found {0}", string.Join(" ,", solutionNames)))
        {
            
        }
    }
}