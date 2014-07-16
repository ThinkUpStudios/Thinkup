using System;
using System.Collections.Generic;
using System.Linq;
using ThinkUp.Sdk.Data;
using ThinkUp.Sdk.Data.Entities;
using ThinkUp.Sdk.Interfaces;

namespace ThinkUp.Sdk.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> userRepository;

        public UserService(IRepository<User> userRepository)
        {
            this.userRepository = userRepository;
        }

        public IEnumerable<IUser> GetAll(string userNameToExclude = null)
        {
            return this.userRepository.GetAll(p => p.Name != userNameToExclude);
        }

        public IEnumerable<IUser> GetAllConnected(string userNameToExclude = null)
        {
            return this.userRepository.GetAll(p => p.IsConnected && p.Name != userNameToExclude);
        }

        public IUser GetByName(string userName)
        {
            return this.userRepository.Get(p => p.Name == userName);
        }

        public IUser GetRandom(string userNameToExclude = null)
        {
            return this.GetAll(userNameToExclude)
                .OrderBy(p => Guid.NewGuid())
                .FirstOrDefault();
        }

        public bool Exist(string userName)
        {
            return this.userRepository.Exist(p => p.Name == userName);
        }

        ///<exception cref="ServiceException">ServiceException</exception>
        public void Connect(string userName, string name = null)
        {
            var existingUser = this.userRepository.Get(p => p.Name == userName);

            try
            {
                if (existingUser == null)
                {
                    var newUser = new User
                    {
                        Name = userName,
                        DisplayName = name ?? userName,
                        IsConnected = true
                    };

                    this.userRepository.Create(newUser);
                }
                else
                {
                    existingUser.IsConnected = true;

                    this.userRepository.Update(existingUser);
                }
            }
            catch (DataException dataEx)
            {
                var actionKeyword = existingUser == null ? "creating" : "updating";
                var errorMessage = string.Format("An error occured when {0} the user {1} to connect", actionKeyword, userName);

                throw new ServiceException(errorMessage, dataEx);
            }
        }

        ///<exception cref="ServiceException">ServiceException</exception>
        public void Disconnect(string userName)
        {
            var existingUser = this.userRepository.Get(p => p.Name == userName);

            if (existingUser == null)
            {
                var errorMessage = string.Format("The user to disconnect, {0}, does not exist", userName);

                throw new ServiceException(errorMessage);
            }

            existingUser.IsConnected = false;

            try
            {
                this.userRepository.Update(existingUser);
            }
            catch (DataException dataEx)
            {
                var errorMessage = string.Format("An error occured when trying to disconnect the user {0}", userName);

                throw new ServiceException(errorMessage, dataEx);
            }
        }
    }
}
