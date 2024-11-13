namespace RagnarockTourGuide.Services.Concrete_Products.Question_CRUD_Repository
{
    public class ReadQuestionRepository
    {
        private readonly string _connectionString;

        public ReadQuestionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
    }
}
