using Splitwise.Data.Infrastructure;
using Splitwise.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Splitwise.Models;
using Splitwise.Model.Exceptions;

namespace Splitwise.Service
{
    public interface IUserService
    {
        void CreateUser(User userToSave);
        void SaveUser();
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            this._userRepository = userRepository;
            this._unitOfWork = unitOfWork;
        }

        public void CreateUser(User userToSave)
        {
            ValidateUserCreate(userToSave);
            this._userRepository.Add(userToSave);            
        }

        private void ValidateUserCreate(User userToSave)
        {
            var parameterErrorCollection = new List<InvalidParameter>();
            ValidationHelper.ValidateRequiredAndAddError(parameterErrorCollection, nameof(userToSave.Username), userToSave.Username);
            ValidationHelper.ValidateRequiredAndAddError(parameterErrorCollection, nameof(userToSave.Email), userToSave.Email);
            ValidationHelper.ValidateRequiredAndAddError(parameterErrorCollection, nameof(userToSave.Password), userToSave.Password);
            ValidationHelper.ValidateRequiredAndAddError(parameterErrorCollection, nameof(userToSave.Currency), userToSave.Currency.ToString());

            if (parameterErrorCollection.Count > 0)
            {
                InvalidParametersException parameterError = new InvalidParametersException(parameterErrorCollection, "Invalid parameters");
                throw parameterError;
            }
        }

        public void SaveUser()
        {
            _unitOfWork.Commit();
        }

        public User GetUser(int id)
        {
            var user = _userRepository.Get(u => u.Id == id);
            return user;
        }
    }
}
