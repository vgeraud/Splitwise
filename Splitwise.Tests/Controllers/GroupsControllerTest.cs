﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Splitwise.Controllers;
using Splitwise.Model;
using Splitwise.Models;
using Splitwise.Models.Enums;
using Splitwise.Service;
using System;
using System.Collections.Generic;
using System.Web.Http.Results;

namespace Splitwise.Tests.Controllers
{
    [TestClass]
    public class GroupsControllerTest
    {
        private GroupsController controller;

        [TestMethod]
        public void PostGroup_CorrectGroup_ReturnsId()
        {
            var fakeGroup = new Group
            {
                Name = "MyGroup",
                Category = GroupCategory.Friends,
                Users = new List<User>()
                {
                    new User { Username = "User1" }
                },
                CurrentBalance = 0
            };

            var fakeSaveResult = new SaveResultModel<Group>
            {
                Model = fakeGroup,
                Success = true
            };

            var groupServiceMock = new Mock<IGroupService>();
            groupServiceMock.Setup(m => m.CreateGroup(It.IsAny<Group>())).Returns(fakeSaveResult);
            controller = new GroupsController(groupServiceMock.Object, null);

            var result = controller.Post(fakeGroup);

            Assert.IsTrue(result != null);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Group>));
        }

        [TestMethod]
        public void PostGroup_NullGroup_ReturnsBadRequest()
        {
            var fakeSaveResult = new SaveResultModel<Group>
            {
                Model = null,
                Success = false
            };

            var groupServiceMock = new Mock<IGroupService>();
            groupServiceMock.Setup(m => m.CreateGroup(It.IsAny<Group>())).Returns(fakeSaveResult);
            controller = new GroupsController(groupServiceMock.Object, null);

            var result = controller.Post(null);

            Assert.IsTrue(result != null);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PostExpense_NullExpense_ReturnBadRequest()
        {
            controller = new GroupsController(null, null);
            var result = controller.PostExpense(10, null);

            Assert.IsTrue(result != null);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PostExpense_IdGroupDosentExist_ReturnNotFound()
        {
            var groupServiceMock = new Mock<IGroupService>();
            groupServiceMock.Setup(m => m.GetGroup(It.IsAny<int>())).Returns<Group>(null);
            controller = new GroupsController(groupServiceMock.Object, null);

            var result = controller.PostExpense(10, new Expense());

            Assert.IsTrue(result != null);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PostExpense_ErrorSaveExpense_ReturnBadRequest()
        {
            var fakeSaveResult = new SaveResultModel<Expense>
            {
                Model = null,
                Success = false,
                ErrorMessages = new List<string> { "error." }
            };

            var expenseServiceMock = new Mock<IExpenseService>();
            var groupServiceMock = new Mock<IGroupService>();
            groupServiceMock.Setup(m => m.GetGroup(It.IsAny<int>())).Returns(new Group());
            expenseServiceMock.Setup(m => m.CreateExpense(It.IsAny<Expense>())).Returns(fakeSaveResult);
            controller = new GroupsController(groupServiceMock.Object, expenseServiceMock.Object);

            var result = controller.PostExpense(10, new Expense());

            Assert.IsTrue(result != null);
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public void PostExpense_CorrectExpense_ReturnOk()
        {
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

            var fakeSaveResult = new SaveResultModel<Expense>
            {
                Model = fakeExpense,
                Success = true
            };

            var expenseServiceMock = new Mock<IExpenseService>();
            var groupServiceMock = new Mock<IGroupService>();
            groupServiceMock.Setup(m => m.GetGroup(It.IsAny<int>())).Returns(new Group());
            expenseServiceMock.Setup(m => m.CreateExpense(It.IsAny<Expense>())).Returns(fakeSaveResult);
            controller = new GroupsController(groupServiceMock.Object, expenseServiceMock.Object);

            var result = controller.PostExpense(10, new Expense());

            Assert.IsTrue(result != null);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Expense>));
        }

        [TestMethod]
        public void PutExpense_NullExpense_ReturnBadRequest()
        {
            controller = new GroupsController(null, null);
            var result = controller.PutExpense(10, 5, null);

            Assert.IsTrue(result != null);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PutExpense_IdGroupDosentExist_ReturnNotFound()
        {
            var groupServiceMock = new Mock<IGroupService>();
            groupServiceMock.Setup(m => m.GetGroup(It.IsAny<int>())).Returns<Group>(null);
            controller = new GroupsController(groupServiceMock.Object, null);

            var result = controller.PutExpense(10, 5, new Expense());

            Assert.IsTrue(result != null);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PutExpense_ErrorUpdateExpense_ReturnBadRequest()
        {
            var fakeSaveResult = new SaveResultModel<Expense>
            {
                Model = null,
                Success = false,
                ErrorMessages = new List<string> { "error." }
            };

            var expenseServiceMock = new Mock<IExpenseService>();
            var groupServiceMock = new Mock<IGroupService>();
            groupServiceMock.Setup(m => m.GetGroup(It.IsAny<int>())).Returns(new Group());
            expenseServiceMock.Setup(m => m.UpdateExpense(It.IsAny<Expense>())).Returns(fakeSaveResult);
            controller = new GroupsController(groupServiceMock.Object, expenseServiceMock.Object);

            var result = controller.PutExpense(10, 5, new Expense());

            Assert.IsTrue(result != null);
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public void PutExpense_CorrectExpense_ReturnOk()
        {
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

            var fakeSaveResult = new SaveResultModel<Expense>
            {
                Model = fakeExpense,
                Success = true
            };

            var expenseServiceMock = new Mock<IExpenseService>();
            var groupServiceMock = new Mock<IGroupService>();
            groupServiceMock.Setup(m => m.GetGroup(It.IsAny<int>())).Returns(new Group());
            expenseServiceMock.Setup(m => m.UpdateExpense(It.IsAny<Expense>())).Returns(fakeSaveResult);
            controller = new GroupsController(groupServiceMock.Object, expenseServiceMock.Object);

            var result = controller.PutExpense(10, 5, new Expense());

            Assert.IsTrue(result != null);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Expense>));
        }
    }
}
