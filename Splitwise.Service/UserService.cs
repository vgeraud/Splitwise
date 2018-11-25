using Splitwise.Data.Infrastructure;
using Splitwise.Data.Repositories;
using System.Collections.Generic;
using Splitwise.Models;
using Splitwise.Model.Validators;
using Splitwise.Model;
using Splitwise.Service.Helpers;
using System;

namespace Splitwise.Service
{
    public interface IUserService
    {
        SaveResultModel<User> CreateUser(User userToSave);

        bool AuthenticateUser(string username, string password);
        SaveResultModel<User> AddFriend(string username, string friend);
        SaveResultModel<User> RemoveFriend(string username, string friend);

        SaveResultModel<User> UpdateUser(User userInfoUpdate);
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

        public User GetUserByUsername(string username)
        {
            var user = _userRepository.Get(u => u.Username == username);
            return user;
        }

        public SaveResultModel<User> UpdateUser(User userInfoUpdate)
        {
            if (string.IsNullOrEmpty(userInfoUpdate.Username))
            {
                return ReturnSimpleErrorResult("invalid username");
            }
            var user = _userRepository.Get(g => g.Username == userInfoUpdate.Username);
            if (user == null)
            {
                return ReturnSimpleErrorResult("user does not exist");
            }
            user.Currency = userInfoUpdate.Currency;
            user.Email = string.IsNullOrEmpty(userInfoUpdate.Email) ? user.Email : userInfoUpdate.Email;
            user.PhoneNumber = string.IsNullOrEmpty(userInfoUpdate.PhoneNumber) ? user.Email : userInfoUpdate.PhoneNumber;
            user.Password = string.IsNullOrEmpty(userInfoUpdate.Password) ? user.Password : SecurePasswordHasher.Hash(userInfoUpdate.Password);
            _unitOfWork.Commit();
            return new SaveResultModel<User> { Success = true, Model = user };
        }

        private SaveResultModel<User> ReturnSimpleErrorResult(string message)
        {
            var result = new SaveResultModel<User> { ErrorMessages = new List<string>() };
            result.Success = false;
            result.ErrorMessages.Add(message);
            return result;
        }

        public bool AuthenticateUser(string username, string password)
        {
            var user = _userRepository.Get(g => g.Username == username);
            return user == null? false : SecurePasswordHasher.Verify(password, user.Password);
        }

        public SaveResultModel<User> AddFriend(string username, string friendUsername)
        {
            var user = GetUserByUsername(username);
            var result = new SaveResultModel<User> { Model = user };
            var friend = GetUserByUsername(friendUsername);

            if (user.Friends == null)
            {
                user.Friends = new List<User>();
            }

            if(friend == null)
            {
                result.ErrorMessages = new List<string> { "The person you are trying to add is not a valid user." };
            }
            else if (!user.Friends.Contains(friend))
            {
                user.Friends.Add(friend);
                this._userRepository.Update(user);
                _unitOfWork.Commit();
                result.Success = true;
            }
            else
            {
                result.ErrorMessages = new List<string> { "The person you are trying to add is already your friend." };
            }

            return result;
        }

        public SaveResultModel<User> RemoveFriend(string username, string friendUsername)
        {
            var user = GetUserByUsername(username);
            var result = new SaveResultModel<User> { Model = user };
            var friend = GetUserByUsername(friendUsername);

            if (user.Friends != null)
            {
                if (friend == null)
                {
                    result.ErrorMessages = new List<string> { "The person you are trying to remove is not a valid user." };
                }
                else if (user.Friends.Contains(friend))
                {
                    user.Friends.Remove(friend);
                    this._userRepository.Update(user);
                    _unitOfWork.Commit();

                    result.Success = true;
                }
                else
                {
                    result.ErrorMessages = new List<string> { "The person you are trying to remove is not your friend." };
                }
            }

            return result;
        }
    }
}
