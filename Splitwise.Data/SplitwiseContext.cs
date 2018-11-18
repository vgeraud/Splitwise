using Splitwise.Data.Configuration;
using Splitwise.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splitwise.Data
{
    public class SplitwiseContext : DbContext
    {
        public SplitwiseContext() : base("SplitwiseContext") { }

        public DbSet<User> User { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Expense> Expenses { get; set; }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ExpenseConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new GroupConfiguration());
        }
    }
}
