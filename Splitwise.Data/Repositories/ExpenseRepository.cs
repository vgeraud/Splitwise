using Splitwise.Data.Infrastracture;
using Splitwise.Data.Infrastructure;
using Splitwise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splitwise.Data.Repositories
{
    public class ExpenseRepository : RepositoryBase<Expense>, IExpenseRepository
    {
        public ExpenseRepository(IDbFactory dbFactory)
            : base(dbFactory) {

        }

    }

    public interface IExpenseRepository : IRepository<Expense>
    {

    }
}
