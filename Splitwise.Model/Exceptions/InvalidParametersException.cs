using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splitwise.Model.Exceptions
{
    public class InvalidParameter
    {
        public string Name { get; set; }
        public string Error { get; set; }
    }

    public class InvalidParametersException : Exception
    {
        private List<InvalidParameter> _parameterErrorCollection;

        public List<InvalidParameter> ParameterErrorCollection { get { return _parameterErrorCollection; } }

        public InvalidParametersException(List<InvalidParameter> parameterErrorCollection, string message) : base(message)
        {
            this._parameterErrorCollection = parameterErrorCollection;
        }        
    }
}
