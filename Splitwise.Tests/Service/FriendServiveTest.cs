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
        public void CreateUser_UserIsCreated()
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

            SaveResultModel<User> result =_userService.AddFriend(userPrincipal, friendToAdd);
            User UserModified = _userService.GetUser(userPrincipal.Id);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(UserModified.Friends.Count == 1);
        }



    }
}
