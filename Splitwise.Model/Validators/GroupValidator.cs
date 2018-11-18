using Splitwise.Models;
using System.Collections.Generic;
using System.Linq;

namespace Splitwise.Model.Validators
{
    public class GroupValidator : IValidator<Group>
    {
        public bool Validate(Group model, out IList<string> errorMessages)
        {
            errorMessages = new List<string>();

            if (string.IsNullOrEmpty(model.Name))
            {
                errorMessages.Add("Group name is required");
            }

            if (errorMessages.Any())
            {
                return false;
            }

            return true;
        }
    }
}
