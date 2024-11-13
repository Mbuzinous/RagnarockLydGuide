using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Enums;
using RagnarockTourGuide.Interfaces.PreviousRepos;

namespace RagnarockTourGuide.Services.PreviousServices
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
            // Husk at bruge hashning til adgangskoder i produktionen for sikkerhed
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT RoleId FROM Users WHERE Email = @Email AND Password = @Password";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password); // Husk at bruge hash i stedet for klartekst

                    var result = cmd.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int roleId))
                    {
                        return (Role)roleId;  // Cast resultatet til Role enum
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
