using Splitwise.Data.Infrastructure;
using Splitwise.Data.Repositories;
using Splitwise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splitwise.Service
{
    public interface IExpenseService
    {
        IEnumerable<Expense> GetExpenses();
        Expense GetExpense(int id);
        void CreateExpense(Expense expense);
        void SaveExpense();

        decimal DoubleUpExpense(int expenseId);
    }

    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ExpenseService(IExpenseRepository expenseRepository, IUnitOfWork unitOfWork)
        {
            this._expenseRepository = expenseRepository;
            this._unitOfWork = unitOfWork;
        }

        #region IExpenseService Members

        public IEnumerable<Expense> GetExpenses()
        {
            var expenses = _expenseRepository.GetAll();
            return expenses;
        }
        
        public Expense GetExpense(int id)
        {
            var expense = _expenseRepository.GetById(id);
            return expense;
        }

        public void CreateExpense(Expense expense)
        {
            _expenseRepository.Add(expense);
        }

        public void SaveExpense()
        {
            _unitOfWork.Commit();
        }


        //ceci est just une function bidon pour illustrer l'utilisation des mocks et l'injection de dépendances. On peut l'enlever après
        public decimal DoubleUpExpense(int expenseId)
        {
            var expense = _expenseRepository.GetById(expenseId);
            decimal result = (decimal) expense.CurrentAmount * 2;
            return result;

        }

        #endregion
    }
}
