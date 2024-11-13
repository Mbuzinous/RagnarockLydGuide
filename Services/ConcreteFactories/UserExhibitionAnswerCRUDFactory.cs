using RagnarockTourGuide.Interfaces.CRUDFactoryInterfaces;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Interfaces.PreviousRepos;
using RagnarockTourGuide.Models;
using RagnarockTourGuide.Services.Concrete_Products.Exhibition_CRUD_Repositories;
using RagnarockTourGuide.Services.Concrete_Products.UserExhibitionAnswer_CRUD_Repository;
using RagnarockTourGuide.Services.CRUDServices;

namespace RagnarockTourGuide.Services.ConcreteFactories
{
    public class UserExhibitionAnswerCRUDFactory : ICRUDFactory<UserExhibitionAnswer>
    {
        private readonly IConfiguration _configuration;
        public UserExhibitionAnswerCRUDFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public ICreateRepository<UserExhibitionAnswer> CreateRepository()
        {
            return new CreateUserExhibitionAnswerRepository(_configuration);
        }

        public IDeleteRepository<UserExhibitionAnswer> DeleteRepository()
        {
            return new DeleteUserExhibitionAnswerRepository(_configuration);
        }

        public IReadRepository<UserExhibitionAnswer> ReadRepository()
        {
            return new ReadUserExhibitionAnswerRepository(_configuration);
        }

        public IUpdateRepository<UserExhibitionAnswer> UpdateRepository()
        {
            return new UpdateUserExhibitionAnswerRepository(_configuration);
        }
    }
}
