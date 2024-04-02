using ScheduSquad.Models;

namespace ScheduSquad.Models
{
    public class User
    {

        public User()
        {

        }

        public User(Guid id, string firstName, string lastName, string email, string password, List<Availability> availabilities)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Availabilities = availabilities;
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Availability> Availabilities { get; set; }

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
