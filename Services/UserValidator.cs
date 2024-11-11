using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RagnarockTourGuide.Enums;
using RagnarockTourGuide.Interfaces;

namespace RagnarockTourGuide.Services
{
    public class UserValidator : IUserValidator
    {
        private readonly string _connectionString;
        public UserValidator(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public Role? ValidateUser(string email, string password)
        {
            // Her skal kodeordet sammenlignes sikkert (fx med hashing) mod det gemte kodeord
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT Role FROM Users WHERE Email = @Email AND Password = @Password";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password); // Husk at bruge hash til rigtige kodeord

                    var result = cmd.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int roleValue))
                    {
                        return (Role)roleValue;
                    }
                }
            }
            return null;
        }
        // Metode til at hente den aktuelle brugers rolle fra sessionen
        public Role GetUserRole(ISession session)
        {
            // Hent brugerens rolle fra sessionen og returner den som Role-enum
            if (session.GetString("UserRole") is string role && Enum.TryParse(role, out Role userRole))
            {
                return userRole;
            }
            return Role.Guest; // Standardrolle, hvis sessionen ikke indeholder brugerens rolle
        }
    }
}
