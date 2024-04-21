using System;
using System.Text;
using System.Security.Cryptography;
using ScheduSquad.Models;

namespace ScheduSquad.Service
{

    public class LoginAuthenticationService : ILoginAuthenticationService
    {

        private readonly IPasswordRepository _passwordRepo;

        public LoginAuthenticationService(IPasswordRepository passwordRepo)
        {
            _passwordRepo = passwordRepo;
        }

        public bool CheckPassword(string password, Guid memberId)
        {
            // Get saved password
            string? savedPassword = _passwordRepo.GetPassword(memberId);
            // If the password is empty, something went wrong lol  ¯\_(ツ)_/¯
            if (!String.IsNullOrEmpty(savedPassword))
            {
                // Get the salt from the db
                string? salt = _passwordRepo.GetSalt(memberId);
                // Hash the input password with the salt
                string hashedPassword = HashPassword(password, salt);
                // Compare. If the saved password matches the hashed password, return true
                if (savedPassword.Trim() == hashedPassword.Trim()) return true;
            }
            // Pass through for failure or null password.
            return false;

        }

        public void UpdatePassword(string password, Guid memberId)
        {
            // Get the existing password from the database.
            string? p = _passwordRepo.GetPassword(memberId);

            // If p is null, generate password and store it
            if (String.IsNullOrEmpty(p))
            {
                var salt = GenerateSalt(); // Make salty
                var hashedPw = HashPassword(password, salt); // Make hashy
                _passwordRepo.UpdatePassword(memberId, password, salt); // Make savey
            }
            else
            {
                // If we had some update password function, it'd be in here.
            }

        }

        private string GenerateSalt()
        {

            int saltSize = 16;

            byte[] saltBytes = new byte[saltSize];

            // Generate the salt using a cryptographic random number generator
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            // Convert the byte array to a base64-encoded string
            string salt = Convert.ToBase64String(saltBytes);

            return salt;
        }

        private string HashPassword(string password, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);

            // Combine password and salt bytes
            byte[] passwordAndSaltBytes = Encoding.UTF8.GetBytes(password).Concat(saltBytes).ToArray();

            // Hash the combined bytes using SHA256
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(passwordAndSaltBytes);

                // Convert the byte array to a base64-encoded string
                string hashedPassword = Convert.ToBase64String(hashedBytes);

                return hashedPassword;
            }
        }
    }
}

