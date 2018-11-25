using Splitwise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splitwise.Model.Validators
{
    public class ExpenseValidator : IValidator<Expense>
    {
        public bool Validate(Expense expense, out IList<string> errorMessages)
        {
            errorMessages = new List<string>();

            if (string.IsNullOrEmpty(expense.Description))
            {
                errorMessages.Add("Expense description is required");
            }

            if (expense.CurrentAmount <= 0.00 || expense.InitialAmount <= 0.00)
            {
                errorMessages.Add("Expense amount is required");
            }

            if (expense.Payer == null)
            {
                errorMessages.Add("Expense payer is required");
            }

            if (errorMessages.Any())
            {
                return false;
            }

            return true;
        }
    }
}
