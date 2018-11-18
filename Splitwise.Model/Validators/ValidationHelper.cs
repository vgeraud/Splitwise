using System.Collections.Generic;

namespace Splitwise.Model.Validators
{
    public static class ValidationHelper
    {
        public static void ValidateRequiredAndAddError(string fieldName, string fieldValue, IList<string> errorCollection)
        {
            if (string.IsNullOrWhiteSpace(fieldValue))
            {
                errorCollection.Add($"{fieldName} is required");
            }
        }
    }
}
