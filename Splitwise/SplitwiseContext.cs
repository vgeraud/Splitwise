using Splitwise.Models;
using System.Data.Entity;

namespace Splitwise
{
    public class SplitwiseContext : DbContext
    {
        public SplitwiseContext() : base("SplitwiseContext")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Expense> Expenses { get; set; }
    }
}