using Splitwise.Data.Infrastructure;
using Splitwise.Data.Repositories;
using System.Collections.Generic;
using Splitwise.Models;
using Splitwise.Model.Validators;
using Splitwise.Model;
using Splitwise.Service.Helpers;

namespace Splitwise.Service
{
    public interface IUserService
    {
        SaveResultModel<User> CreateUser(User userToSave);

        bool AuthenticateUser(string username, string password);
        SaveResultModel<User> AddFriend(User user, User friend);
        SaveResultModel<User> RemoveFriend(User user, User friend);
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

            userToSave.Password = SecurePasswordHasher.Hash(userToSave.Password);

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

        public bool AuthenticateUser(string username, string password)
        {
            var user = _userRepository.Get(g => g.Username == username);
            return user == null? false : SecurePasswordHasher.Verify(password, user.Password);
        }

        public SaveResultModel<User> AddFriend(User user, User friend)
        {
            var result = new SaveResultModel<User> { Model = user };

            if(user.Friends == null)
            {
                user.Friends = new List<User>();
            }
            user.Friends.Add(friend);
            this._userRepository.Update(user);
            _unitOfWork.Commit();

            result.Success = true;

            return result;
        }

        public SaveResultModel<User> RemoveFriend(User user, User friend)
        {
            var result = new SaveResultModel<User> { Model = user };

            if (user.Friends != null)
            {
                user.Friends.Remove(friend);
                this._userRepository.Update(user);
                _unitOfWork.Commit();

                result.Success = true;
            }

            return result;
        }
    }
}
