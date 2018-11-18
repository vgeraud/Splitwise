using Splitwise.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace Splitwise.Controllers
{
    public class UserController : ApiController
    {
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
