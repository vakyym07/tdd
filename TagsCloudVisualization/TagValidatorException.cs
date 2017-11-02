using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class TagValidatorException : Exception
    {
        public TagValidatorException(string message)
            : base(message)
        {
        }
    }
}
