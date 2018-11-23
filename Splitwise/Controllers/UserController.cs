using Splitwise.Models;
using Splitwise.Service;
using System;
using System.Security.Claims;
using System.Web.Http;
using System.Linq;

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

        [HttpPost]
        [Authorize]
        public IHttpActionResult UpdateUser(User user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest();
                }

                var saveResult = _userService.UpdateUser(user);


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

        private string GetUsernameInSession()
        {
            var identity = User.Identity as ClaimsIdentity;
            Claim identityClaim = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return identityClaim?.Value;
        }
    }
}
