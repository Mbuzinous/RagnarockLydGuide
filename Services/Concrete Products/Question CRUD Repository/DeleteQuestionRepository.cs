﻿using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Services.Concrete_Products.Question_CRUD_Repository
{
    public class DeleteQuestionRepository : IDeleteRepository<Question>
    {
        private readonly string _connectionString;

        public DeleteQuestionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task DeleteAsync(DeleteParameter parameter)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM Questions WHERE Id = @QuestionId;";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@QuestionId", parameter.Id);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
