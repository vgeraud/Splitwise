using Splitwise.Models;
using Splitwise.Service;
using System.Web.Http;

namespace Splitwise.Controllers
{
    public class GroupsController : ApiController
    {
        private IGroupService _groupService;
        private IExpenseService _expenseService;

        public GroupsController(IGroupService groupService, IExpenseService expenseService)
        {
            _groupService = groupService;
            _expenseService = expenseService;
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

        [HttpPost]
        [Route("groups/{id}/expenses")]
        public IHttpActionResult PostExpense(int id, Expense expense)
        {
            if(expense == null)
            {
                return BadRequest();
            }

            var group = _groupService.GetGroup(id);

            if(group == null)
            {
                return NotFound();
            }

            var saveResult = _expenseService.CreateExpense(expense);

            if(!saveResult.Success)
            {
                return BadRequest(string.Join(". ", saveResult.ErrorMessages));
            }

            group.expenses.Add(saveResult.Model);

            //Updater group
            return null;
        }
    }
}