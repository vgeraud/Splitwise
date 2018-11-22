﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void AddFriend_CreateLinkBetweenUserAndFriend()
        {
            User userPrincipal = new User();
            userPrincipal.Id = 1;
            userPrincipal.Username = "userPrincipal";
            userPrincipal.Email = "userPrincipal@server.com";
            userPrincipal.PhoneNumber = "514111111";

            User friendToAdd = new User();
            friendToAdd.Id = 2;
            friendToAdd.Username = "friendUser";
            friendToAdd.Email = "friendUser@server.com";
            friendToAdd.PhoneNumber = "514111111";

            SetupMocks(new List<User> {
                userPrincipal
                });
            _userService.CreateUser(userPrincipal);

            SaveResultModel<User> result = _userService.AddFriend(userPrincipal, friendToAdd);
            User UserModified = _userService.GetUser(userPrincipal.Id);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(UserModified.Friends.Count == 1);
        }


        [TestMethod]
        public void AddFriend_Validate_IfLinkBetweenFriendAndUserAlreadyExist()
        {
            User userPrincipal = new User();
            userPrincipal.Id = 1;
            userPrincipal.Username = "userPrincipal";
            userPrincipal.Email = "userPrincipal@server.com";
            userPrincipal.PhoneNumber = "514111111";

            User friendToAdd = new User();
            friendToAdd.Id = 2;
            friendToAdd.Username = "friendUser";
            friendToAdd.Email = "friendUser@server.com";
            friendToAdd.PhoneNumber = "514111111";

            userPrincipal.Friends = new List<User>();
            userPrincipal.Friends.Add(friendToAdd);

            SetupMocks(new List<User> {
                userPrincipal
                });
            _userService.CreateUser(userPrincipal);

            SaveResultModel<User> result = _userService.AddFriend(userPrincipal, friendToAdd);
            User UserModified = _userService.GetUser(userPrincipal.Id);

            Assert.IsFalse(result.Success);
            Assert.IsTrue(UserModified.Friends.Count == 1);
        }

        [TestMethod]
        public void RemoveFriend_EliminateLinkBetweenUserAndFriend()
        {
            User userPrincipal = new User();
            userPrincipal.Id = 1;
            userPrincipal.Username = "userPrincipal";
            userPrincipal.Email = "userPrincipal@server.com";
            userPrincipal.PhoneNumber = "514111111";

            User friendToAdd = new User();
            friendToAdd.Id = 2;
            friendToAdd.Username = "friendUser";
            friendToAdd.Email = "friendUser@server.com";
            friendToAdd.PhoneNumber = "514111111";

            userPrincipal.Friends = new List<User>();
            userPrincipal.Friends.Add(friendToAdd);

            SetupMocks(new List<User> {
                userPrincipal
                });
            _userService.CreateUser(userPrincipal);

            SaveResultModel<User> result = _userService.RemoveFriend(userPrincipal, friendToAdd);
            User UserModified = _userService.GetUser(userPrincipal.Id);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(UserModified.Friends.Count == 0);
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
