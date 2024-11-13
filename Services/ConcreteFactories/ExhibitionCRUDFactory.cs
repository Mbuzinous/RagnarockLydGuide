using RagnarockTourGuide.Models;
using RagnarockTourGuide.Interfaces.CRUDFactoryInterfaces;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Interfaces.PreviousRepos;
using RagnarockTourGuide.Services.Concrete_Products.Exhibition_CRUD_Repositories;
using RagnarockTourGuide.Services.CRUDServices;

namespace RagnarockTourGuide.Services.ConcreteFactories
{
    public class ExhibitionCRUDFactory : ICRUDFactory<Exhibition>
    {
        private readonly IConfiguration _configuration;
        private IFileHandler<IFormFile> _fileRepository;
        public ExhibitionCRUDFactory(IConfiguration configuration, IFileHandler<IFormFile> fileRepository)
        {
            _configuration = configuration;
            _fileRepository = fileRepository;
        }
        public ICreateRepository<Exhibition> CreateRepository()
        {
            return new CreateExhibitionRepository(_configuration, _fileRepository);
        }

        public IDeleteRepository<Exhibition> DeleteRepository()
        {
            return new DeleteExhibitionRepository(_configuration, _fileRepository);
        }

        public IReadRepository<Exhibition> ReadRepository()
        {
            return new ReadExhibitionRepository(_configuration, _fileRepository);
        }

        public IUpdateRepository<Exhibition> UpdateRepository()
        {
            return new UpdateExhibitionRepository(_configuration, _fileRepository);
        }
    }
}
