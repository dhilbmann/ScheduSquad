using ScheduSquad.Models;

namespace ScheduSquad.Models
{
    public class Member : IPersisitedEntityBase
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        private string _firstName;
        public string FirstName { get { return _firstName; } }

        private string _lastName;
        public string LastName { get { return _lastName; } }
        
        private string _email; 
        public string Email { get { return _email; } }
 
        public List<Availability> Availabilities { get; set; }

        public Member() {
            _firstName = String.Empty;
            _lastName = String.Empty;
            _email = String.Empty;
            Availabilities = new List<Availability>();
        }

        public Member(Guid id, string firstName, string lastName, string email, List<Availability> availabilities)
        {
            Id = id;
            _firstName = firstName;
            _lastName = lastName;
            _email = email;
            Availabilities = availabilities;
        }

        public Member(Guid id, string firstName, string lastName, string email)
        {
            Id = id;
            _firstName = firstName;
            _lastName = lastName;
            _email = email;
            Availabilities = new List<Availability>();
        }

        public Member(string firstName, string lastName, string email)
        {
            Id = Guid.NewGuid();
            _firstName = firstName;
            _lastName = lastName;
            _email = email;
            Availabilities = new List<Availability>();
        }

        public Member(string firstName, string lastName, string email, string password) : this(Guid.NewGuid(), firstName, lastName, email, new List<Availability>()) { }

        
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
