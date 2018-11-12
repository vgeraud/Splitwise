using Splitwise.Models;
using Splitwise.Service;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace Splitwise.Controllers
{
    public class ExpenseController : ApiController
    {
        private IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        // GET api/expense
        [HttpGet]
        public Expense Get(int id)
        {
            return _expenseService.GetExpense(id);
        }     
    }
}