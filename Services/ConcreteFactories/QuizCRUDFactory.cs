using RagnarockTourGuide.Interfaces.CRUDFactoryInterfaces;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Models;
using RagnarockTourGuide.Services.Concrete_Products.Quiz_CRUD_Repositories;

namespace RagnarockTourGuide.Services.ConcreteFactories
{
    public class QuizCRUDFactory : ICRUDFactory<Quiz>
    {
        private readonly IConfiguration _configuration;
        public QuizCRUDFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ICreateRepository<Quiz> CreateRepository()
        {
            return new CreateQuizRepository(_configuration);
        }

        public IDeleteRepository<Quiz> DeleteRepository()
        {
            return new DeleteQuizRepository(_configuration);
        }

        public IReadRepository<Quiz> ReadRepository()
        {
            return new ReadQuizRepository(_configuration);
        }

        public IUpdateRepository<Quiz> UpdateRepository()
        {
            return new UpdateQuizRepository(_configuration);
        }
    }
}
