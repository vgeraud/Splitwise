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
        void AddExpense(Group group, Expense expense);
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

        public void AddExpense(Group group, Expense expense)
        {
            group.expenses.Add(expense);

            _groupRepository.Update(group);
            _unitOfWork.Commit();
        }
    }
}
