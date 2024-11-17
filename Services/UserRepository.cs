using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Interfaces;
using RagnarockTourGuide.Models;
using RagnarockTourGuide.Models.Enums;

namespace RagnarockTourGuide.Services
{
    public class UserRepository : ICRUDRepository<User>
    {
        private readonly string _connectionString;
        private readonly string _audioFileTarget = "userImages";


        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task CreateAsync(User user)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
            INSERT INTO Users (Name, Email, Password, RoleId)
            VALUES (@Name, @Email, @Password, @RoleId)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Password", user.Password);  // Husk at hash, hvis nødvendigt
                cmd.Parameters.AddWithValue("@RoleId", (int)user.Role);
                try
                {
                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
                catch (SqlException ex)
                {
                    // Log fejlmeddelelsen her
                    Console.WriteLine($"SQL Error: {ex.Message}");
                    throw;  // Eller håndter fejlen efter behov
                }
            }
        }
        public async Task DeleteAsync(DeleteParameter parameter)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "EXEC DeleteUser @CurrentUserRole, @TargetUserRole, @TargetUserId";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@CurrentUserRole", parameter.CurrentUserRole);
                cmd.Parameters.AddWithValue("@TargetUserRole", parameter.TargetUserRole);
                cmd.Parameters.AddWithValue("@TargetUserId", parameter.Id);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }

        }
        public async Task<User> GetByIdAsync(int id)
        {
            User user = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Users WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Email = reader["Email"].ToString(),
                            Password = reader["Password"].ToString(),
                            Role = (Role)(int)reader["RoleId"],
                            ImageFileName = reader["ImageFileName"].ToString()
                        };
                    }
                }
            }

            return user;
        }

        public async Task<List<User>> GetAllAsync()
        {
            var users = new List<User>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Users";

                SqlCommand cmd = new SqlCommand(query, conn);
                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Email = reader["Email"].ToString(),
                            Password = reader["Password"].ToString(),
                            Role = (Role)(int)reader["RoleId"],
                            ImageFileName = reader["ImageFileName"].ToString()
                        });
                    }
                }
            }

            return users;
        }


        public List<User> FilterListByNumber(List<User> listofT, int filterNumber)
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> GetUsedNumbersAsync()
        {
            throw new NotImplementedException();
        }
        public async Task UpdateAsync(User user)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    UPDATE Users SET Name = @Name, Email = @Email, Password = @Password, 
                        RoleId = @RoleId, ImageFileName = @ImageFileName 
                    WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", user.Id);
                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@RoleId", (int)user.Role);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }



    }
}
