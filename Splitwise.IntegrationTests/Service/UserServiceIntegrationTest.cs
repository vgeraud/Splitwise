﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Splitwise.Data.Infrastructure;
using Splitwise.Data.Repositories;
using Splitwise.Model.Validators;
using Splitwise.Models;
using Splitwise.Service;
using System;

namespace Splitwise.IntegrationTests.Service
{
    [TestClass]
    public class UserServiceIntegrationTest
    {
        //[TestMethod] are commented out to avoid VS to find them. The tests should only be run when required

       //Uncomment to include it as a test
       // [TestMethod]
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
                UserService service = new UserService(new UserRepository(factory), new UnitOfWork(factory), new UserValidator());
                service.CreateUser(userToSave);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
