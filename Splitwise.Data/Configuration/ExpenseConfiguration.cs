using Splitwise.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splitwise.Data.Configuration
{
    public class ExpenseConfiguration : EntityTypeConfiguration<Expense>
    {
        public ExpenseConfiguration()
        {
            ToTable("Expenses");
            Property(g => g.Id).IsRequired();
            Property(g => g.Type).IsRequired();
            Property(g => g.Date).IsRequired();
        }
    }
}
