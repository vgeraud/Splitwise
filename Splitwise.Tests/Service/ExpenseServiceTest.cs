using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Splitwise.Data.Infrastructure;
using Splitwise.Data.Repositories;
using Splitwise.Model.Validators;
using Splitwise.Models;
using Splitwise.Models.Enums;
using Splitwise.Service;
using System;
using System.Linq.Expressions;

namespace Splitwise.Tests.Service
{
    [TestClass]
    public class ExpenseServiceTest
    {
        private IValidator<Expense> expenseValidator => new ExpenseValidator();

        [TestMethod]
        public void CreateExpense_InvalidModel_DescriptionEmpty_ReturnsError()
        {
            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            var fakeExpense = new Expense
            {
                CurrentAmount = 1,
                InitialAmount = 1,
                Payer = new User()
            };

            var service = new ExpenseService(expenseRepositoryMock.Object, Mock.Of<IUnitOfWork>(), expenseValidator);
            var saveResult = service.CreateExpense(fakeExpense);

            Assert.IsTrue(saveResult != null);
            Assert.IsFalse(saveResult.Success);
            Assert.IsTrue(saveResult.ErrorMessages[0] == "Expense description is required");
        }

        [TestMethod]
        public void CreateExpense_InvalidModel_AmountEmpty_ReturnsError()
        {
            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            var fakeExpense = new Expense
            {
                Description = "tmp",
                Payer = new User()
            };

            var service = new ExpenseService(expenseRepositoryMock.Object, Mock.Of<IUnitOfWork>(), expenseValidator);
            var saveResult = service.CreateExpense(fakeExpense);

            Assert.IsTrue(saveResult != null);
            Assert.IsFalse(saveResult.Success);
            Assert.IsTrue(saveResult.ErrorMessages[0] == "Expense amount is required");
        }

        [TestMethod]
        public void CreateExpense_InvalidModel_PayerEmpty_ReturnsError()
        {
            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            var fakeExpense = new Expense
            {
                Description = "tmp",
                CurrentAmount = 1,
                InitialAmount = 1
            };

            var service = new ExpenseService(expenseRepositoryMock.Object, Mock.Of<IUnitOfWork>(), expenseValidator);
            var saveResult = service.CreateExpense(fakeExpense);

            Assert.IsTrue(saveResult != null);
            Assert.IsFalse(saveResult.Success);
            Assert.IsTrue(saveResult.ErrorMessages[0] == "Expense payer is required");
        }

        [TestMethod]
        public void CreateExpense_ExistingExpense_ReturnsError()
        {
            var fakeExpense = new Expense
            {
                Description = "tmp",
                CurrentAmount = 1,
                InitialAmount = 1,
                Payer = new User()
            };

            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            expenseRepositoryMock.Setup(m => m.Get(It.IsAny<Expression<Func<Expense, bool>>>())).Returns(fakeExpense);

            var service = new ExpenseService(expenseRepositoryMock.Object, Mock.Of<IUnitOfWork>(), expenseValidator);
            var saveResult = service.CreateExpense(fakeExpense);

            Assert.IsTrue(saveResult != null);
            Assert.IsFalse(saveResult.Success);
            Assert.IsTrue(saveResult.ErrorMessages[0] == "A expense with the same description, amount and payer already exists.");
        }

        [TestMethod]
        public void CreateExpense_ValidModel()
        {
            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            var fakeExpense = new Expense
            {
                Description = "tmp",
                Type = ExpenseType.Entertainment,
                Date = DateTime.Now,
                Currency = Currency.CAD,
                IsTaxIncluded = true,
                CurrentAmount = 1,
                InitialAmount = 1,
                Payer = new User()
            };

            var service = new ExpenseService(expenseRepositoryMock.Object, Mock.Of<IUnitOfWork>(), expenseValidator);
            var saveResult = service.CreateExpense(fakeExpense);

            Assert.IsTrue(saveResult != null);
            Assert.IsTrue(saveResult.Success);
            Assert.IsTrue(saveResult.ErrorMessages.Count == 0);
        }

    }
}
