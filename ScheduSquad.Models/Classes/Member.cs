using ScheduSquad.Models;

namespace ScheduSquad.Models
{
    public class Member : PersisitedEntityBase
    {

        private string _firstName;
        public string FirstName { get { return _firstName; } }

        private string _lastName;
        public string LastName { get { return _lastName; } }
        
        private string _email; 
        public string Email { get { return _email; } }

        private string _password;
        public string Password { get { return _password; } }
 
        public List<Availability> Availabilities { get; set; }

        public Member() {
            _firstName = String.Empty;
            _lastName = String.Empty;
            _email = String.Empty;
            _password = String.Empty;
            Availabilities = new List<Availability>();
        }

        public Member(Guid id, string firstName, string lastName, string email, string password, List<Availability> availabilities)
        {
            base.Id = id;
            _firstName = firstName;
            _lastName = lastName;
            _email = email;
            _password = password;
            Availabilities = availabilities;
        }

        public Member(Guid id, string firstName, string lastName, string email, string password)
        {
            base.Id = id;
            _firstName = firstName;
            _lastName = lastName;
            _email = email;
            _password = password;
            Availabilities = new List<Availability>();
        }

        public Member(string firstName, string lastName, string email, string password) : this(Guid.NewGuid(), firstName, lastName, email, password, new List<Availability>()) { }

        
        /// <summary>
        /// Itterates over Availabilities and adds each availabilities code to a list
        /// </summary>
        /// <returns>List of distinct availability codes for the user</returns>
        public List<int> GetAllAvailabilityCodes()
        {
            List<int> codes = new List<int>();
            foreach (Availability a in Availabilities)
            {
                codes.AddRange(a.GetAvailabilityCodes());
            }
            return codes.Distinct().ToList();
        }

    }
}
