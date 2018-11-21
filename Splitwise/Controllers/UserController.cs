using Splitwise.Models;
using Splitwise.Service;
using System;
using System.Web.Http;

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
        [Authorize]
        public IHttpActionResult CreateUser(User user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest();
                }

                var saveResult = _userService.CreateUser(user);


                if (saveResult.Success)
                {
                    saveResult.Model.Password = "";
                    return Ok(saveResult.Model);
                }

                return BadRequest(string.Join(". ", saveResult.ErrorMessages));
            }           
            catch (Exception ex)
            {
                //TODO: log exception
                return this.InternalServerError(ex);             
            }
            
        }


        // GET api/user
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var model =  new User
            {
                Id = id,
                Username = "test"
            };

            return Ok(model);
        }
    }
}
