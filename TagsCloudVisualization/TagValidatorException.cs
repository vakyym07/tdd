using System;

namespace TagsCloudVisualization
{
    internal class TagValidatorException : Exception
    {
        public TagValidatorException(string message)
            : base(message)
        {
        }
    }
}