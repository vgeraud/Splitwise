using Splitwise.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace Splitwise.Controllers
{
    public class UserController : ApiController
    {
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
