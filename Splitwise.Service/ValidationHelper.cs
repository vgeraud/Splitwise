using Splitwise.Model.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splitwise.Service
{
    public static class ValidationHelper
    {
        public static void ValidateRequiredAndAddError(List<InvalidParameter> invalidParameterCollection, string fieldName, string fieldValue)
        {
            if (string.IsNullOrWhiteSpace(fieldValue))
            {
                invalidParameterCollection.Add(new InvalidParameter { Name = fieldName, Error = $"{fieldName} is required" });
            }
        }
    }
}
