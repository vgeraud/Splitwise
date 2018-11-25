using Splitwise.Data.Infrastructure;
using Splitwise.Data.Repositories;
using Splitwise.Model;
using Splitwise.Model.Validators;
using Splitwise.Models;
using System.Collections.Generic;

namespace Splitwise.Service
{
    public interface IExpenseService
    {
        SaveResultModel<Expense> CreateExpense(Expense expense);
        SaveResultModel<Expense> UpdateExpense(Expense expense);
    }

    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<Expense> _expenseValidator;

        public ExpenseService(IExpenseRepository expenseRepository, IUnitOfWork unitOfWork, IValidator<Expense> expenseValidator)
        {
            this._expenseRepository = expenseRepository;
            this._unitOfWork = unitOfWork;
            this._expenseValidator = expenseValidator;
        }

        public SaveResultModel<Expense> CreateExpense(Expense model)
        {
            if (!_expenseValidator.Validate(model, out var errorMessages))
            {
                return new SaveResultModel<Expense>
                {
                    Success = false,
                    Model = model,
                    ErrorMessages = errorMessages
                };
            }

            var alreadyExists = _expenseRepository.Get(g => g.Description == model.Description
                                                        && g.CurrentAmount == model.CurrentAmount
                                                        && g.InitialAmount == model.InitialAmount
                                                        && g.Payer == model.Payer) != null;

            if (alreadyExists)
            {
                return new SaveResultModel<Expense>
                {
                    Success = false,
                    Model = model,
                    ErrorMessages = new List<string> { "A expense with the same description, amount and payer already exists." }
                };
            }

            _expenseRepository.Add(model);
            _unitOfWork.Commit();

            return new SaveResultModel<Expense>
            {
                Success = true,
                Model = model,
                ErrorMessages = errorMessages
            };
        }

        public SaveResultModel<Expense> UpdateExpense(Expense model)
        {
            if (!_expenseValidator.Validate(model, out var errorMessages))
            {
                return new SaveResultModel<Expense>
                {
                    Success = false,
                    Model = model,
                    ErrorMessages = errorMessages
                };
            }

            var expense = _expenseRepository.GetById(model.Id);

            if(expense == null)
            {
                return new SaveResultModel<Expense>
                {
                    Model = null,
                    Success = false,
                    ErrorMessages = new List<string> { "Expense does not exist." }
                };
            }

            expense.Description = model.Description;
            expense.Type = model.Type;
            expense.Currency = model.Currency;
            expense.Date = model.Date;
            expense.IsTaxIncluded = model.IsTaxIncluded;
            expense.CurrentAmount = model.CurrentAmount;
            expense.InitialAmount = model.InitialAmount;
            expense.Payer = model.Payer;

            _expenseRepository.Update(expense);
            _unitOfWork.Commit();

            return new SaveResultModel<Expense>
            {
                Model = expense,
                Success = true,
                ErrorMessages = errorMessages
            };
        }
    }
}
