using Splitwise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splitwise.Model.Validators
{
    public class UserValidator : IValidator<User>
    {
        public bool Validate(User model, out IList<string> errorMessages)
        {
            errorMessages = new List<string>();

            ValidationHelper.ValidateRequiredAndAddError(nameof(model.Username), model.Username, errorMessages);
            ValidationHelper.ValidateRequiredAndAddError(nameof(model.Email), model.Email, errorMessages);
            ValidationHelper.ValidateRequiredAndAddError(nameof(model.Password), model.Password, errorMessages);
            ValidationHelper.ValidateRequiredAndAddError(nameof(model.Currency), model.Currency.ToString(), errorMessages);
          
            if (errorMessages.Any())
            {
                return false;
            }

            return true;
        }
    }
}
