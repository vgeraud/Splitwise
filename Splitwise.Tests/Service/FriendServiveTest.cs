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
using System.Collections.Generic;
using System.Data.Entity;

namespace Splitwise.Tests.Service
{
    [TestClass]
    public class FriendServiceTest
    {

        Mock<IDbFactory> _factoryMock;
        Mock<SplitwiseContext> _contextMock;
        Mock<DbSet<User>> _dataMock;
        UserRepository _userRepository;
        UserService _userService;

       [TestMethod]
        public void AddFriend()
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

            SaveResultModel<User> result =_userService.AddFriend(userPrincipal, friendToAdd);
            User UserModified = _userService.GetUser(userPrincipal.Id);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(UserModified.Friends.Count == 1);
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
