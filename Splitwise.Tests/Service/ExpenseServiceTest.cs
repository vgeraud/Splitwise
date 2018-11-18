using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Splitwise.Data.Infrastructure;
using Splitwise.Data.Repositories;
using Splitwise.Models;
using Splitwise.Service;

namespace Splitwise.Tests.Service
{
    [TestClass]
    public class ExpenseServiceTest
    {
        [TestMethod]
        public void DoubleUpExpenseShouldBeDouble()
        {
            var expenseRepository = new Mock<IExpenseRepository>();
            var fakeExpense = new Expense { CurrentAmount = 4 };
            expenseRepository.Setup(r => r.GetById(1)).Returns(fakeExpense);

            IExpenseService service = new ExpenseService(expenseRepository.Object, Mock.Of<IUnitOfWork>());
            decimal result = service.DoubleUpExpense(1);
            Assert.AreEqual(result, 8);
        }
    }
}
