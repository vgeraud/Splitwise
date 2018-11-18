using Microsoft.VisualStudio.TestTools.UnitTesting;
using Splitwise.Data.Infrastructure;
using Splitwise.Data.Repositories;
using Splitwise.Models;
using Splitwise.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splitwise.IntegrationTests.Service
{
    [TestClass]
    public class UserServiceIntegrationTest
    {

        [TestMethod]
        public void ShouldSaveUser()
        {
            User userToSave = new User();
            userToSave.Id = 2;
            userToSave.Username = "lcardona2";
            userToSave.Password = "somePwd";
            userToSave.Email = "email@server.com";
            userToSave.Currency = Models.Enums.Currency.CAD;
            userToSave.PhoneNumber = "51878928";
            
            try
            {
                DbFactory factory = new DbFactory();
                UserService service = new UserService(new UserRepository(factory), new UnitOfWork(factory));
                service.CreateUser(userToSave);
                service.SaveUser();
            }
            catch (Exception ex)
            {
                throw;
            }
         


        }

      
    }
}
