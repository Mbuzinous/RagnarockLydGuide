using Kojg_Ragnarock_Guide.Interfaces;
using Kojg_Ragnarock_Guide.Models;
using Kojg_Ragnarock_Guide.Services;
using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Enums;
using RagnarockTourGuide.Interfaces.PreviousRepos;
using RagnarockTourGuide.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RagnarockTourGuide.Services.PreviousServices
{
    public class UserCRUDRepository : IUserCRUDRepository<User>
    {
        IFIleRepository<IFormFile> _fileRepository;

        private readonly string _connectionString;
        private readonly string _audioFileTarget = "userImages";


        public UserCRUDRepository(IConfiguration configuration, IFIleRepository<IFormFile> fileRepository)
        {
            _fileRepository = fileRepository;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task CreateAsync(User user)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
            INSERT INTO Users (Name, Email, Password, RoleId, ImageFileName)
            VALUES (@Name, @Email, @Password, @RoleId, @ImageFileName)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Password", user.Password);  // Husk at hash, hvis nødvendigt
                cmd.Parameters.AddWithValue("@RoleId", (int)user.Role);
                cmd.Parameters.AddWithValue("@ImageFileName", await _fileRepository.SaveFileAsync(user.ImageFile, _audioFileTarget));

                try
                {
                    conn.Open();
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

        public User GetById(int id)
        {
            User user = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Users WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
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

        public List<User> GetAll()
        {
            var users = new List<User>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Users";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
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

        public async Task UpdateAsync(User user, User notUsed)
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
                cmd.Parameters.AddWithValue("@ImageFileName", await _fileRepository.SaveFileAsync(user.ImageFile, _audioFileTarget));

                conn.Open();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        // Delete a user by ID
        public void Delete(int currentUserRole, int targetUserRole, int targetUserId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {


                string query = "EXEC DeleteUser @CurrentUserRole, @TargetUserRole, @TargetUserId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CurrentUserRole", currentUserRole);
                cmd.Parameters.AddWithValue("@TargetUserRole", targetUserRole);
                cmd.Parameters.AddWithValue("@TargetUserId", targetUserId);

                conn.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }

                catch (SqlException sqlEx)
                {
                    if (sqlEx.Number == 50003)
                    {
                        Console.WriteLine("Unauthorized: Cannot delete a user with the same or higher role.");
                    }
                    else
                    {
                        Console.WriteLine($"SQL Error: {sqlEx.Message}");
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                    throw;
                }

            }
        }

        public List<User> FilterListByNumber(List<User> listofT, int filterNumber)
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> GetUsedNumbersAsync()
        {
            throw new NotImplementedException();
        }

    }
}
