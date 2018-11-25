using Splitwise.Data.Infrastructure;
using Splitwise.Data.Repositories;
using Splitwise.Model;
using Splitwise.Model.Validators;
using Splitwise.Models;
using System.Collections.Generic;

namespace Splitwise.Service
{
    public interface IGroupService
    {
        SaveResultModel<Group> CreateGroup(Group model);
        SaveResultModel<Group> ModifyGroup(Group model);
        int? DeleteGroup(int groupId);
        void AddExpense(Group group, Expense expense);
        void UpdateExpense(Group group, Expense expense);
        Group GetGroup(int id);

    }

    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<Group> _groupValidator;

        public GroupService(IGroupRepository groupRepository, IUnitOfWork unitOfWork, IValidator<Group> groupValidator)
        {
            _groupRepository = groupRepository;
            _unitOfWork = unitOfWork;
            _groupValidator = groupValidator;
        }

        public Group GetGroup(int id) {
            return _groupRepository.GetById(id);
        }

        public SaveResultModel<Group> CreateGroup(Group model)
        {
            if (!_groupValidator.Validate(model, out var errorMessages))
            {
                return new SaveResultModel<Group>
                {
                    Success = false,
                    Model = model,
                    ErrorMessages = errorMessages
                };
            }

            var nameAlreadyExists = _groupRepository.Get(g => g.Name == model.Name) != null;

            if (nameAlreadyExists)
            {
                return new SaveResultModel<Group>
                {
                    Success = false,
                    Model = model,
                    ErrorMessages = new List<string> { "A group with the same name already exists." }
                };
            }

            _groupRepository.Add(model);
            _unitOfWork.Commit();
            
            return new SaveResultModel<Group>
            {
                Success = true,
                Model = model,
                ErrorMessages = errorMessages
            };
        }

        public SaveResultModel<Group> ModifyGroup(Group model)
        {
            if (!_groupValidator.Validate(model, out var errorMessages))
            {
                return new SaveResultModel<Group>
                {
                    Model = null,
                    Success = false,
                    ErrorMessages = errorMessages
                };   
            }

            var groupInDb = _groupRepository.GetById(model.Id);

            if (groupInDb == null)
            {
                return new SaveResultModel<Group>
                {
                    Model = null,
                    Success = false,
                    ErrorMessages = new List<string> { "Group does not exist." }
                };
            }

            groupInDb.Name = model.Name;
            groupInDb.Category = model.Category;
            groupInDb.CurrentBalance = model.CurrentBalance;
            groupInDb.Users = model.Users;

            _groupRepository.Update(model);
            _unitOfWork.Commit();

            return new SaveResultModel<Group>
            {
                Model = groupInDb,
                Success = true,
                ErrorMessages = errorMessages
            };
        }

        public int? DeleteGroup(int groupId)
        {
            var groupInDb = _groupRepository.GetById(groupId);

            if (groupInDb == null)
            {
                return null;
            }

            _groupRepository.Delete(groupInDb);
            _unitOfWork.Commit();

            return groupInDb.Id;
        }

        public void AddExpense(Group group, Expense expense)
        {
            group.expenses.Add(expense);

            _groupRepository.Update(group);
            _unitOfWork.Commit();
        }

        public void UpdateExpense(Group group, Expense expense)
        {
            int objectToChange = -1;
            foreach (Expense exps in group.expenses) 
            {
                if(exps.Id == expense.Id)
                {
                    objectToChange = group.expenses.IndexOf(exps);
                    break;
                }
            }

            group.expenses[objectToChange] = expense;

            _groupRepository.Update(group);
            _unitOfWork.Commit();
        }
    }
}
