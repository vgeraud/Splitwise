using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Splitwise.Data.Infrastructure;
using Splitwise.Data.Repositories;
using Splitwise.Model.Validators;
using Splitwise.Models;
using Splitwise.Models.Enums;
using Splitwise.Service;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Splitwise.Tests.Service
{
    [TestClass]
    public class GroupServiceTest
    {
        private IValidator<Group> groupValidator => new GroupValidator();

        [TestMethod]
        public void CreateGroup_ValidModel_ReturnsSuccessTrue()
        {
            var groupRepositoryMock = new Mock<IGroupRepository>();
            var fakeGroup = new Group
            {
                Name = "MyGroup",
                Category = GroupCategory.Entertainment,
                CurrentBalance = 0,
                Users = new List<User> { new User { Username = "Test" } }
            };

            var service = new GroupService(groupRepositoryMock.Object, Mock.Of<IUnitOfWork>(), groupValidator);
            var saveResult = service.CreateGroup(fakeGroup);

            Assert.IsTrue(saveResult != null);
            Assert.IsTrue(saveResult.Success);
            Assert.IsTrue(saveResult.ErrorMessages.Count == 0);
        }

        [TestMethod]
        public void CreateGroup_InvalidModel_ReturnsSuccessFalse()
        {
            var groupRepositoryMock = new Mock<IGroupRepository>();
            var fakeGroup = new Group
            {
                CurrentBalance = 0
            };
            
            var service = new GroupService(groupRepositoryMock.Object, Mock.Of<IUnitOfWork>(), groupValidator);
            var saveResult = service.CreateGroup(fakeGroup);

            Assert.IsTrue(saveResult != null);
            Assert.IsFalse(saveResult.Success);
            Assert.IsTrue(saveResult.ErrorMessages.Count == 1);
        }

        [TestMethod]
        public void CreateGroup_ExistingGroup_ReturnsSucessFalse()
        {
            var fakeGroup = new Group
            {
                Name = "MyGroup"
            };

            var groupRepositoryMock = new Mock<IGroupRepository>();
            groupRepositoryMock.Setup(m => m.Get(It.IsAny<Expression<Func<Group, bool>>>())).Returns(fakeGroup);
            
            var service = new GroupService(groupRepositoryMock.Object, Mock.Of<IUnitOfWork>(), groupValidator);
            var saveResult = service.CreateGroup(fakeGroup);

            Assert.IsTrue(saveResult != null);
            Assert.IsFalse(saveResult.Success);
            Assert.IsTrue(saveResult.ErrorMessages.Count == 1);
        }

        [TestMethod]
        public void DeleteGroup_ExistingGroup_ReturnsId()
        {
            var groupId = 1;

            var groupRepositoryMock = new Mock<IGroupRepository>();
            groupRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(new Group { Id = groupId, Name = "MyName" });

            var service = new GroupService(groupRepositoryMock.Object, Mock.Of<IUnitOfWork>(), Mock.Of<IValidator<Group>>());
            var result = service.DeleteGroup(groupId);

            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(result, groupId);
        }

        [TestMethod]
        public void DeleteGroup_NotExistingGroup_ReturnsNull()
        {
            var groupId = 1;

            var groupRepositoryMock = new Mock<IGroupRepository>();
            groupRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns((Group)null);

            var service = new GroupService(groupRepositoryMock.Object, Mock.Of<IUnitOfWork>(), Mock.Of<IValidator<Group>>());
            var result = service.DeleteGroup(groupId);

            Assert.IsFalse(result.HasValue);
        }
    }
}
