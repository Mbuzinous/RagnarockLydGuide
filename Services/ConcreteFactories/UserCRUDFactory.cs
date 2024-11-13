using RagnarockTourGuide.Interfaces.CRUDFactoryInterfaces;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Interfaces.PreviousRepos;
using RagnarockTourGuide.Models;
using RagnarockTourGuide.Services.Concrete_Products;
using RagnarockTourGuide.Services.Concrete_Products.User_CRUD_Repositories;

namespace RagnarockTourGuide.Services.ConcreteFactories
{
    public class UserCRUDFactory : ICRUDFactory<User>
    {
        private readonly IConfiguration _configuration;
        private IFileHandler<IFormFile> _fileRepository;
        public UserCRUDFactory(IConfiguration configuration, IFileHandler<IFormFile> fileRepository)
        {
            _configuration = configuration;
            _fileRepository = fileRepository;
        }

        public ICreateRepository<User> CreateRepository()
        {
            return new CreateUserRepository(_configuration, _fileRepository);
        }

        public IDeleteRepository<User> DeleteRepository()
        {
            return new DeleteUserRepository(_configuration, _fileRepository);
        }

        public IReadRepository<User> ReadRepository()
        {
            return new ReadUserRepository(_configuration, _fileRepository);
        }

        public IUpdateRepository<User> UpdateRepository()
        {
            return new UpdateUserRepository(_configuration, _fileRepository);
        }
    }
}
