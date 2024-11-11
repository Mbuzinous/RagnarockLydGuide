using Kojg_Ragnarock_Guide.Interfaces;
using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Enums;
using RagnarockTourGuide.Interfaces;
using RagnarockTourGuide.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RagnarockTourGuide.Services
{
    public class UserCRUDRepository : IUserCRUDRepository<User>
    {
        private readonly string _connectionString;
        private readonly IFIleRepository<IFormFile> _fileRepository;
        private readonly string _imageFileTarget = "userImages";

        public UserCRUDRepository(IConfiguration configuration, IFIleRepository<IFormFile> fileRepository)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _fileRepository = fileRepository;
        }

        // Create a new user
        public async Task CreateAsync(User toBeCreatedUser)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Users (Name, Email, Password, Role, ImageFileName) " +
                               "VALUES (@Name, @Email, @Password, @Role, @ImageFileName)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", toBeCreatedUser.Name);
                cmd.Parameters.AddWithValue("@Email", toBeCreatedUser.Email);
                cmd.Parameters.AddWithValue("@Password", toBeCreatedUser.Password);
                cmd.Parameters.AddWithValue("@Role", (int)toBeCreatedUser.Role);
                cmd.Parameters.AddWithValue("@ImageFileName", await _fileRepository.SaveFileAsync(toBeCreatedUser.ImageFile, _imageFileTarget));

                conn.Open();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        // Get a user by ID
        public User GetById(int id)
        {
            User userToGet = null;

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
                        userToGet = new User
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Email = reader["Email"].ToString(),
                            Password = reader["Password"].ToString(),
                            Role = (Role)Convert.ToInt32(reader["Role"]),
                            ImageFileName = reader["ImageFileName"].ToString()
                        };
                    }
                }
            }

            return userToGet;
        }

        // Get all users
        public List<User> GetAll()
        {
            List<User> listOfAllUsers = new List<User>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Users";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listOfAllUsers.Add(new User
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Email = reader["Email"].ToString(),
                            Password = reader["Password"].ToString(),
                            Role = (Role)Convert.ToInt32(reader["Role"]),
                            ImageFileName = reader["ImageFileName"].ToString()
                        });
                    }
                }
            }

            return listOfAllUsers;
        }

        // Update an existing user
        public async Task UpdateAsync(User toBeUpdatedUser, User oldUser)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Users SET Name = @Name, Email = @Email, Password = @Password, " +
                               "Role = @Role, ImageFileName = @ImageFileName WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", toBeUpdatedUser.Id);
                cmd.Parameters.AddWithValue("@Name", toBeUpdatedUser.Name);
                cmd.Parameters.AddWithValue("@Email", toBeUpdatedUser.Email);
                cmd.Parameters.AddWithValue("@Password", toBeUpdatedUser.Password);
                cmd.Parameters.AddWithValue("@Role", (int)toBeUpdatedUser.Role);
                cmd.Parameters.AddWithValue("@ImageFileName", await _fileRepository.UpdateFileAsync(toBeUpdatedUser.ImageFile, oldUser.ImageFileName, _imageFileTarget));

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
