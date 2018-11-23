using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Splitwise.Data;
using Splitwise.Data.Infrastracture;
using Splitwise.Data.Infrastructure;
using Splitwise.Data.Repositories;
using Splitwise.Model;
using Splitwise.Model.Validators;
using Splitwise.Models;
using Splitwise.Service;
using Splitwise.Service.Helpers;
using System.Collections.Generic;
using System.Data.Entity;

namespace Splitwise.Tests.Service
{
    [TestClass]
    public class UserServiceTest
    {

        Mock<IDbFactory> _factoryMock;
        Mock<SplitwiseContext> _contextMock;
        Mock<DbSet<User>> _dataMock;
        UserRepository _userRepository;
        UserService _userService;

       [TestMethod]
        public void CreateUser_UserIsCreated()
        {
            string originalPwd = "somePwd";
            User userToSave = new User();
            userToSave.Id = 1;
            userToSave.Username = "lcardona";
            userToSave.Email = "email@server.com";
            userToSave.Password = originalPwd;
            userToSave.Currency = Models.Enums.Currency.CAD;
            userToSave.PhoneNumber = "51878928";

            List<User> userTable = new List<User>();

            SetupMocks(new List<User>());

            SaveResultModel<User> result = _userService.CreateUser(userToSave);
            User createdUser = _userService.GetUser(userToSave.Id);

            _dataMock.Verify(m => m.Add(It.IsAny<User>()), Times.Once);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(createdUser);
            Assert.AreEqual(createdUser.Username, userToSave.Username);
            Assert.IsTrue(SecurePasswordHasher.Verify(originalPwd, createdUser.Password));
            Assert.AreEqual(createdUser.Currency, userToSave.Currency);
            Assert.AreEqual(createdUser.PhoneNumber, userToSave.PhoneNumber);
        }

        [TestMethod]
        public void CreateUser_Validate_ShouldDetectInvalidInput()
        {
            SetupMocks(new List<User>());
            User userToSave = new User();
            SaveResultModel<User> result = _userService.CreateUser(userToSave);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void CreateUser_Validate_ShouldDetectExistingUser()
        {
            SetupMocks(new List<User> { 
                new User() { 
                    Id = 101,
                    Username = "lcardona",
                    Password = "somePwd",
                    Currency = Models.Enums.Currency.CAD,
                    PhoneNumber = "51878928"}
                });
            User userToSave = new User();
            userToSave.Id = 1;
            userToSave.Username = "lcardona";
            userToSave.Password = "somePwd";
            userToSave.Currency = Models.Enums.Currency.CAD;
            userToSave.PhoneNumber = "51878928";
            SaveResultModel<User> result = _userService.CreateUser(userToSave);
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void AuthenticateUser_ShouldAuthenticateUser()
        {
            var username = "lcardona";
            var password = "pwd123";
            SetupMocks(new List<User> {
                new User() {
                    Username = username,
                    Password = SecurePasswordHasher.Hash(password),
                    }
                });
            Assert.IsTrue(_userService.AuthenticateUser(username, password));
        }

        [TestMethod]
        public void AuthenticateUser_ShouldNotAuthenticateUser()
        {
            var username = "lcardona";
            var password = "pwd123";
            SetupMocks(new List<User> {
                new User() {
                    Username = username,
                    Password = SecurePasswordHasher.Hash(password),
                    }
                });
            Assert.IsFalse(_userService.AuthenticateUser(username, "differentPwd"));
        }


        [TestMethod]
        public void UpdateUser_ShouldUpdateUser()
        {
            User userInfoUpdate = new User();
            userInfoUpdate.Username = "lcardona";
            userInfoUpdate.Email = "email@server.com";
            userInfoUpdate.Currency = Models.Enums.Currency.CAD;
            userInfoUpdate.Password = "new password!";

            User existingUser = new User();
            existingUser.Username = "lcardona";
            existingUser.PhoneNumber = "43143679";
            existingUser.Currency = Models.Enums.Currency.USD;

            SetupMocks(new List<User> {
                existingUser
                });

            var result = _userService.UpdateUser(userInfoUpdate);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.Model.Email, userInfoUpdate.Email);
            Assert.AreEqual(result.Model.Currency, userInfoUpdate.Currency);
            Assert.AreEqual(result.Model.PhoneNumber, existingUser.PhoneNumber);
            Assert.IsTrue(SecurePasswordHasher.Verify(userInfoUpdate.Password, result.Model.Password));
        }

        private void SetupMocks(List<User> data)
        {
            _factoryMock = new Mock<IDbFactory>();
            _contextMock = new Mock<SplitwiseContext>();
            _dataMock = TestHelper.GetQueryableMockDbSet<User>(data);
            _contextMock.Setup(c => c.Set<User>()).Returns(_dataMock.Object);
            _factoryMock.Setup(f => f.Init()).Returns(_contextMock.Object);

            _userRepository = new UserRepository(_factoryMock.Object);
            _userService = new UserService(_userRepository, Mock.Of<IUnitOfWork>(), new UserValidator());
        }



    }
}
