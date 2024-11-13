using RagnarockTourGuide.Interfaces.CRUDFactoryInterfaces;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Interfaces.PreviousRepos;
using RagnarockTourGuide.Models;
using RagnarockTourGuide.Services.Concrete_Products.Question_CRUD_Repository;

namespace RagnarockTourGuide.Services.ConcreteFactories
{
    public class QuestionCRUDFactory : ICRUDFactory<Question>
    {
        private readonly IConfiguration _configuration;
        public QuestionCRUDFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public ICreateRepository<Question> CreateRepository()
        {
            return new CreateQuestionRepository(_configuration);
        }

        public IDeleteRepository<Question> DeleteRepository()
        {
            return new DeleteQuestionRepository(_configuration);
        }

        public IReadRepository<Question> ReadRepository()
        {
            return new ReadQuestionRepository(_configuration);
        }

        public IUpdateRepository<Question> UpdateRepository()
        {
            return new UpdateQuestionRepository(_configuration);
        }
    }
}
