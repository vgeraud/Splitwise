using Splitwise.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Splitwise.Data
{
    public class SplitwiseSeedData : DropCreateDatabaseIfModelChanges<SplitwiseContext>
    {
        protected override void Seed(SplitwiseContext context)
        {
            GetExpenses().ForEach(g => context.Expenses.Add(g));

            context.Commit();
        }
        
        private static List<Expense> GetExpenses()
        {
            return new List<Expense>
            {
                new Expense {
                    Id = 1,
                    Description = "Expense 1",
                    Date = DateTime.Now.AddDays(-10),
                    InitialAmount = 100,
                    CurrentAmount = 46,
                    Currency = Models.Enums.Currency.CAD,
                    IsTaxIncluded = true
                },

                new Expense {
                    Id = 2,
                    Description = "Expense 2",
                    Date = DateTime.Now.AddDays(-23),
                    InitialAmount = 55,
                    CurrentAmount = 55,
                    Currency = Models.Enums.Currency.CAD,
                    IsTaxIncluded = true
                },

                new Expense {
                    Id = 3,
                    Description = "Expense 3",
                    Date = DateTime.Now.AddDays(-45),
                    InitialAmount = 50,
                    CurrentAmount = 44,
                    Currency = Models.Enums.Currency.CAD,
                    IsTaxIncluded = true
                }
            };
        }
    }
}
