using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Enums;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Interfaces.PreviousRepos;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Services.Concrete_Products.User_CRUD_Repositories
{
    public class ReadUserRepository : IReadRepository<User>
    {
        IFileHandler<IFormFile> _fileRepository;

        private readonly string _connectionString;
        private readonly string _audioFileTarget = "userImages";


        public ReadUserRepository(IConfiguration configuration, IFileHandler<IFormFile> fileRepository)
        {
            _fileRepository = fileRepository;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task <User> GetByIdAsync(int id)
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


    }
}
