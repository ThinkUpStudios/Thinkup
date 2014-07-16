using ThinkUp.Sdk.Interfaces;

namespace ThinkUp.Sdk.Data.Entities
{
    public class User : DataEntity, IUser
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool IsConnected { get; set; }

        public User(string userName)
        {
            this.Name = userName;
        }

        public User()
        {
        }
    }
}
