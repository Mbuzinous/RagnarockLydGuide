namespace RagnarockTourGuide.Services.Concrete_Products.Question_CRUD_Repository
{
    public class DeleteQuestionRepository
    {
        private readonly string _connectionString;

        public DeleteQuestionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
    }
}
