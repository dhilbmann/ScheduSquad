using ScheduSquad.Models;
using System.Configuration.Assemblies;
using System.Security.Cryptography.X509Certificates;


namespace ScheduSquad.Models
{
    public class Squad
    {
        private List<User> _users;
        public List<User> Users { get { return _users; } }

        public Squad(List<User> users)
        {
            _users = users;
        }

        public Squad()
        {
            _users = new List<User>();
        }

        public List<string> GetAllAvailabilityDescriptions()
        {
            return new List<string>();
        }

        public void AddUser(User user)
        {
            // check to see if the user exists; if they don't, add user, else throw an exception
            if (!_users.Exists(x => x.Id == user.Id)) _users.Add(user);
            else
                throw new Exception("Unable to add.  User already exists in the squad.");
        }

        public void RemoveUser(User user)
        {
            if (_users.Exists(x => x.Id == user.Id)) _users.Remove(user);
            else
                throw new Exception("Unable to remove.  User doesn't exist in the squad.");
        }
    }
}
