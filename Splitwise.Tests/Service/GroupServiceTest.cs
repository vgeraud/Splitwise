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
        public void GetGroup_InvalideId_ReturnNull()
        {
            var groupRepositoryMock = new Mock<IGroupRepository>();
            groupRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns<Group>(null);
            var service = new GroupService(groupRepositoryMock.Object, Mock.Of<IUnitOfWork>(), groupValidator);

            var group = service.GetGroup(12);

            Assert.IsNull(group);
        }

        [TestMethod]
        public void GetGroup_ValideId_ReturnModel()
        {
            var groupRepositoryMock = new Mock<IGroupRepository>();
            var fakeGroup = new Group
            {
                Name = "MyGroup",
                Category = GroupCategory.Entertainment,
                CurrentBalance = 0,
                Users = new List<User> { new User { Username = "Test" } }
            };
            groupRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(fakeGroup);

            var service = new GroupService(groupRepositoryMock.Object, Mock.Of<IUnitOfWork>(), groupValidator);
            var group = service.GetGroup(12);

            Assert.IsNotNull(group);
        }

        [TestMethod]
        public void CreateGroup_ValidModel_ReturnsId()
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

        [TestMethod]
        public void ModifyGroup_ExistingGroup_ReturnsUpdatedSaveModel()
        {
            var fakeGroup = new Group { Id = 1, Name = "MyGroup", Category = GroupCategory.Entertainment };

            var groupRepositoryMock     = new Mock<IGroupRepository>();
            groupRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(new Group { Id = 1, Name = "OldGroup", Category = GroupCategory.Friends });

            var service = new GroupService(groupRepositoryMock.Object, Mock.Of<IUnitOfWork>(), groupValidator);
            var result = service.ModifyGroup(fakeGroup);

            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.Model.Id, fakeGroup.Id);
            Assert.AreEqual(result.Model.Name, fakeGroup.Name);
            Assert.AreEqual(result.Model.Category, fakeGroup.Category);
            Assert.IsTrue(result.ErrorMessages.Count == 0);

        }

        [TestMethod]
        public void ModifyGroup_NotExistingGroup_ReturnsSuccessFalse()
        {
            var fakeGroup = new Group { Id = 1, Name = "MyGroup", Category = GroupCategory.Entertainment };

            var groupRepositoryMock = new Mock<IGroupRepository>();
            groupRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns((Group)null);

            var service = new GroupService(groupRepositoryMock.Object, Mock.Of<IUnitOfWork>(), groupValidator);
            var result = service.ModifyGroup(fakeGroup);

            Assert.IsTrue(result != null);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.Model, null);
            Assert.IsTrue(result.ErrorMessages.Count == 1);
        }

        [TestMethod]
        public void ModifyGroup_InvalidGroupModel_ReturnsSuccessFalse()
        {
            var fakeGroup = new Group { Id = 1, CurrentBalance = 0 };

            var groupRepositoryMock = new Mock<IGroupRepository>();
            groupRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).Returns(new Group { Id = 1, Name = "OldGroup", Category = GroupCategory.Friends });

            var service = new GroupService(groupRepositoryMock.Object, Mock.Of<IUnitOfWork>(), groupValidator);
            var result = service.ModifyGroup(fakeGroup);

            Assert.IsTrue(result != null);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.Model, null);
            Assert.IsTrue(result.ErrorMessages.Count == 1);
        }
    }
}
