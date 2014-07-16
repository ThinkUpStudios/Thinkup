using System.Collections.Generic;
using ThinkUp.Sdk.Interfaces;

namespace ThinkUp.Sdk.Services
{
    public interface IUserService
    {
        IEnumerable<IUser> GetAll(string userNameToExclude = null);

        IEnumerable<IUser> GetAllConnected(string userNameToExclude = null);

        IUser GetByName(string userName);

        IUser GetRandom(string userNameToExclude = null);

        bool Exist(string userName);

        ///<exception cref="ServiceException">ServiceException</exception>
        void Connect(string userName, string name = null);

        ///<exception cref="ServiceException">ServiceException</exception>
        void Disconnect(string userName);
    }
}
