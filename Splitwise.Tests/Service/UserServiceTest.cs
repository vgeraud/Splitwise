using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Splitwise.Data;
using Splitwise.Data.Infrastracture;
using Splitwise.Data.Infrastructure;
using Splitwise.Data.Repositories;
using Splitwise.Model.Exceptions;
using Splitwise.Models;
using Splitwise.Service;
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
        public void SaveUser_UserIsCreated()
        {
            User userToSave = new User();
            userToSave.Id = 1;
            userToSave.Username = "lcardona";
            userToSave.Password = "somePwd";
            userToSave.Currency = Models.Enums.Currency.CAD;
            userToSave.PhoneNumber = "51878928";
            
            List<User> userTable = new List<User>();

            SetupMocks(new List<User>());
                        
            _userService.CreateUser(userToSave);
            _userService.SaveUser();
            User createdUser = _userService.GetUser(userToSave.Id);

            _dataMock.Verify(m => m.Add(It.IsAny<User>()), Times.Once);
            Assert.IsNotNull(createdUser);
            Assert.AreEqual(createdUser.Username, userToSave.Username);
            Assert.AreEqual(createdUser.Password, userToSave.Password);
            Assert.AreEqual(createdUser.Currency, userToSave.Currency);
            Assert.AreEqual(createdUser.PhoneNumber, userToSave.PhoneNumber);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidParametersException))]
        public void SaveUser_Validate_ShouldDetectInvalidInput()
        {
            SetupMocks(new List<User>());
            User userToSave = new User();
            _userService.CreateUser(userToSave);
        }
      
        private void SetupMocks(List<User> data)
        {
            _factoryMock = new Mock<IDbFactory>();
            _contextMock = new Mock<SplitwiseContext>();
            _dataMock = TestHelper.GetQueryableMockDbSet<User>(data);
            _contextMock.Setup(c => c.Set<User>()).Returns(_dataMock.Object);
            _factoryMock.Setup(f => f.Init()).Returns(_contextMock.Object);

            _userRepository = new UserRepository(_factoryMock.Object);
            _userService = new UserService(_userRepository, Mock.Of<IUnitOfWork>());
        }



    }
}
