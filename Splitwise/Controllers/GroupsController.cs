using Splitwise.Models;
using Splitwise.Service;
using System.Web.Http;

namespace Splitwise.Controllers
{
    public class GroupsController : ApiController
    {
        private IGroupService _groupService;

        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpPost]
        public IHttpActionResult Post(Group groupModel)
        {
            if (groupModel == null)
            {
                return BadRequest();
            }

            var saveResult = _groupService.CreateGroup(groupModel);

            if (saveResult.Success)
            {
                return Ok(saveResult.Model);
            }

            return BadRequest(string.Join(". ", saveResult.ErrorMessages));
        }
    }
}