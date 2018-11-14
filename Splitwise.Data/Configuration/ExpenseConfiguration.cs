using Splitwise.Models;
using System.Data.Entity.ModelConfiguration;

namespace Splitwise.Data.Configuration
{
    public class ExpenseConfiguration : EntityTypeConfiguration<Expense>
    {
        public ExpenseConfiguration()
        {
            ToTable("Expenses");
            Property(g => g.Id).IsRequired();
            Property(g => g.Date).IsRequired();
            Property(g => g.InitialAmount).IsRequired();
            Property(g => g.IsTaxIncluded).IsRequired();
        }
    }
}
