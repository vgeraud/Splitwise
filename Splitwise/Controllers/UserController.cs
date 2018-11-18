using Newtonsoft.Json;
using Splitwise.Helpers;
using Splitwise.Model.Exceptions;
using Splitwise.Models;
using Splitwise.Service;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Splitwise.Controllers
{
    public class UserController : ApiController
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public HttpResponseMessage CreateUser(User user)
        {
            try
            {
                _userService.CreateUser(user);
                _userService.SaveUser();

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (InvalidParametersException invalidParams)
            {
                return ResponseHelper.ResponseFromInvalidParametersException(invalidParams);
            }
            catch (Exception ex)
            {
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(ex.Message);
                return response;
            }
            
        }


        // GET api/user
        [HttpGet]
        public User Get(int id)
        {
            return new User
            {
                Id = id,
                Username = "test"
            };
        }
    }
}
