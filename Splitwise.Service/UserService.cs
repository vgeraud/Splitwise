using Splitwise.Data.Infrastructure;
using Splitwise.Data.Repositories;
using System.Collections.Generic;
using Splitwise.Models;
using Splitwise.Model.Validators;
using Splitwise.Model;

namespace Splitwise.Service
{
    public interface IUserService
    {
        SaveResultModel<User> CreateUser(User userToSave);
        SaveResultModel<User> AddFriend(User user, User friend);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<User> _userValidator;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IValidator<User> userValidator)
        {
            this._userRepository = userRepository;
            this._unitOfWork = unitOfWork;
            this._userValidator = userValidator;
        }

        public SaveResultModel<User> CreateUser(User userToSave)
        {
            var result = new SaveResultModel<User> { Model = userToSave };

            IList<string> errorMessages = new List<string>();
            if (!_userValidator.Validate(userToSave, out errorMessages))
            {
                result.Success = false;
                result.ErrorMessages = errorMessages;
                return result;
            }

            var nameAlreadyExists = _userRepository.Get(g => g.Username == userToSave.Username) != null;

            if (nameAlreadyExists)
            {
                result.Success = false;
                result.ErrorMessages = new List<string> { "Username already exists." };
                return result;
            }

            this._userRepository.Add(userToSave);
            _unitOfWork.Commit();

            result.Success = true;
            return result;
        }

        public User GetUser(int id)
        {
            var user = _userRepository.Get(u => u.Id == id);
            return user;
        }

        public SaveResultModel<User> AddFriend(User user, User friend)
        {
            var result = new SaveResultModel<User> { Model = user };

            user.Friends.Add(friend);
            this._userRepository.Update(user);
            _unitOfWork.Commit();

            result.Success = true;

            return result;
        }
    }
}
