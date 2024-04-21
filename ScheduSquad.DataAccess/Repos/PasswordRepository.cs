using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using ScheduSquad.Models;

namespace ScheduSquad.DataAccess;
public class PasswordRepository : IPasswordRepository
{
    private IDbConfiguration _dbConfiguration;


    public PasswordRepository(IDbConfiguration dbConfiguration)
    {
        _dbConfiguration = dbConfiguration;
    }

    public string? GetPassword(Guid memberId)
    {
        // Maybe there isn't a password?  Treat this variable as nullable until set by reader
        string? password = null;

        // using connection
        using (SqlConnection connection = (SqlConnection)_dbConfiguration.GetDbConnection())
        {
            // Open connection
            connection.Open();
            // Create & using command
            using (SqlCommand command = new SqlCommand("SELECT TOP 1 PwHash FROM Users WHERE UserPk = @memberId", connection))
            {
                // Set a parameter for memberId
                command.Parameters.AddWithValue("@memberId", memberId.ToString());
                // Query returns a single value (top 1) single field; executeScalar instead of reader
                password = command.ExecuteScalar().ToString();
            }
        } // Connection should close on dispose
        // Returns the password, whether it was found in the db or not.
        return password;
    }

    // Separated GetSalt and GetPassword.  Something felt bad about returning these two values together.
    public string? GetSalt(Guid memberId)
    {
        string? salt = null;

        // using connection
        using (SqlConnection connection = (SqlConnection)_dbConfiguration.GetDbConnection())
        {
            // Open connection
            connection.Open();
            // Create & using command
            using (SqlCommand command = new SqlCommand("SELECT TOP 1 PwSalt FROM Users WHERE UserPk = @memberId", connection))
            {
                // Set a parameter for memberId
                command.Parameters.AddWithValue("@memberId", memberId.ToString());
                // Query returns a single value (top 1) single field; executeScalar instead of reader
                salt = command.ExecuteScalar().ToString();
            }
        } // Connection should close on dispose
        // Returns the salt, whether it was found in the db or not.
        return salt;
    }


    public void UpdatePassword(Guid memberId, string password, string salt)
    {
        using (SqlConnection connection = (SqlConnection)_dbConfiguration.GetDbConnection())
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("INSERT INTO Users (PwHash, PwSalt) VALUES (@PwHash, @PwSalt) WHERE UserPk = @memberId ", connection))
            {
                command.Parameters.AddWithValue("@PwHash", password);
                command.Parameters.AddWithValue("@PwSalt", salt);
                command.Parameters.AddWithValue("@memberId", memberId);

                command.ExecuteNonQuery();
            }
        } // Connection should close on dispose
    }
}
