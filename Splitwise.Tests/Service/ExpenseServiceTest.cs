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

        [TestMethod]
        public void UpdateExpense_InvalidModel_DescriptionEmpty_ReturnsError()
        {
            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            var fakeExpense = new Expense
            {
                CurrentAmount = 1,
                InitialAmount = 1,
                Payer = new User()
            };

            var service = new ExpenseService(expenseRepositoryMock.Object, Mock.Of<IUnitOfWork>(), expenseValidator);
            var saveResult = service.UpdateExpense(fakeExpense);

            Assert.IsTrue(saveResult != null);
            Assert.IsFalse(saveResult.Success);
            Assert.IsTrue(saveResult.ErrorMessages[0] == "Expense description is required");
        }

        [TestMethod]
        public void UpdateExpense_InvalidModel_AmountEmpty_ReturnsError()
        {
            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            var fakeExpense = new Expense
            {
                Description = "tmp",
                Payer = new User()
            };

            var service = new ExpenseService(expenseRepositoryMock.Object, Mock.Of<IUnitOfWork>(), expenseValidator);
            var saveResult = service.UpdateExpense(fakeExpense);

            Assert.IsTrue(saveResult != null);
            Assert.IsFalse(saveResult.Success);
            Assert.IsTrue(saveResult.ErrorMessages[0] == "Expense amount is required");
        }

        [TestMethod]
        public void UpdateExpense_InvalidModel_PayerEmpty_ReturnsError()
        {
            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            var fakeExpense = new Expense
            {
                Description = "tmp",
                CurrentAmount = 1,
                InitialAmount = 1
            };

            var service = new ExpenseService(expenseRepositoryMock.Object, Mock.Of<IUnitOfWork>(), expenseValidator);
            var saveResult = service.UpdateExpense(fakeExpense);

            Assert.IsTrue(saveResult != null);
            Assert.IsFalse(saveResult.Success);
            Assert.IsTrue(saveResult.ErrorMessages[0] == "Expense payer is required");
        }

        [TestMethod]
        public void UpdateExpense_ExpenseDoesNotExiste_ReturnsError()
        {
            var fakeExpense = new Expense
            {
                Id = 3,
                Description = "tmp",
                CurrentAmount = 1,
                InitialAmount = 1,
                Payer = new User()
            };

            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            expenseRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns((Expense) null);

            var service = new ExpenseService(expenseRepositoryMock.Object, Mock.Of<IUnitOfWork>(), expenseValidator);
            var saveResult = service.UpdateExpense(fakeExpense);

            Assert.IsTrue(saveResult != null);
            Assert.IsFalse(saveResult.Success);
            Assert.IsTrue(saveResult.ErrorMessages[0] == "Expense does not exist.");
        }

        [TestMethod]
        public void UpdateExpense_ValidModel()
        {
            var updateExpense = new Expense
            {
                Id = 5,
                Description = "new desc",
                Type = ExpenseType.Gas,
                Currency = Currency.CAD,
                Date = DateTime.Now,
                IsTaxIncluded = true,
                CurrentAmount = 5,
                InitialAmount = 1,
                Payer = new User()
            };

            var fakeExpense = new Expense
            {
                Id = 5,
                Description = "tmp",
                Type = ExpenseType.Entertainment,
                Date = DateTime.Now,
                Currency = Currency.CAD,
                IsTaxIncluded = true,
                CurrentAmount = 1,
                InitialAmount = 1,
                Payer = new User()
            };

            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            expenseRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns((Expense)fakeExpense);

            var service = new ExpenseService(expenseRepositoryMock.Object, Mock.Of<IUnitOfWork>(), expenseValidator);
            var saveResult = service.UpdateExpense(updateExpense);

            Assert.IsTrue(saveResult != null);
            Assert.IsTrue(saveResult.Success);
            Assert.AreEqual(saveResult.Model.Id, updateExpense.Id);
            Assert.AreEqual(saveResult.Model.Description, updateExpense.Description);
            Assert.AreEqual(saveResult.Model.Type, updateExpense.Type);
            Assert.AreEqual(saveResult.Model.Currency, updateExpense.Currency);
            Assert.AreEqual(saveResult.Model.Date, updateExpense.Date);
            Assert.AreEqual(saveResult.Model.IsTaxIncluded, updateExpense.IsTaxIncluded);
            Assert.AreEqual(saveResult.Model.CurrentAmount, updateExpense.CurrentAmount);
            Assert.AreEqual(saveResult.Model.InitialAmount, updateExpense.InitialAmount);
            Assert.AreEqual(saveResult.Model.Payer, updateExpense.Payer);
            Assert.IsTrue(saveResult.ErrorMessages.Count == 0);
        }

    }
}
