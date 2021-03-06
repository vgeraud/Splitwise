﻿using Splitwise.Models;
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
        [Authorize]
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

        [HttpPut]
        [Authorize]
        public IHttpActionResult Put(Group groupModel)
        {
            if (groupModel == null)
            {
                return BadRequest();
            }


            var saveResult = _groupService.ModifyGroup(groupModel);

            if (saveResult.Success)
            {
                return Ok(saveResult.Model);
            }

            return BadRequest(string.Join(". ", saveResult.ErrorMessages));
        }

        [HttpDelete]
        [Authorize]
        public IHttpActionResult Delete(int groupId)
        {
            var isDeleted = _groupService.DeleteGroup(groupId) != null;

            if (!isDeleted)
            {
                return BadRequest("Does not exist.");
            }

            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("groups/{id}/expenses")]
        public IHttpActionResult PostExpense(int id, Expense expense)
        {
            if (expense == null)
            {
                return BadRequest();
            }

            var group = _groupService.GetGroup(id);

            if (group == null)
            {
                return NotFound();
            }

            var saveResult = _expenseService.CreateExpense(expense);

            if (!saveResult.Success)
            {
                return BadRequest(string.Join(". ", saveResult.ErrorMessages));
            }

            _groupService.AddExpense(group, saveResult.Model);
            return Ok(saveResult.Model);
        }

        [HttpPut]
        [Route("groups/{groupId}/expenses/{expenseId}")]
        public IHttpActionResult PutExpense(int groupId, int expenseId, Expense expense)
        {
            if (expense == null)
            {
                return BadRequest();
            }

            var group = _groupService.GetGroup(groupId);

            if (group == null)
            {
                return NotFound();
            }

            expense.Id = expenseId;
            var saveResult = _expenseService.UpdateExpense(expense);

            if (!saveResult.Success)
            {
                return BadRequest(string.Join(". ", saveResult.ErrorMessages));
            }

            _groupService.AddExpense(group, saveResult.Model);
            return Ok(saveResult.Model);
        }
    }
}