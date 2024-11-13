using RagnarockTourGuide.Interfaces.CRUDFactoryInterfaces;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Interfaces.PreviousRepos;
using RagnarockTourGuide.Models;
using RagnarockTourGuide.Services.Concrete_Products.Exhibition_CRUD_Repositories;
using RagnarockTourGuide.Services.Concrete_Products.ExhibitionQuestion_CRUD_Repository;
using RagnarockTourGuide.Services.Concrete_Products.ExhibitionQuestionCRUDRepository;
using RagnarockTourGuide.Services.CRUDServices;

namespace RagnarockTourGuide.Services.ConcreteFactories
{
    public class ExhibitionQuestionCRUDFactory : ICRUDFactory<ExhibitionQuestion>
    {
        private readonly IConfiguration _configuration;
        public ExhibitionQuestionCRUDFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public ICreateRepository<ExhibitionQuestion> CreateRepository()
        {
            return new CreateExhibitionQuestionRepository(_configuration);
        }

        public IDeleteRepository<ExhibitionQuestion> DeleteRepository()
        {
            return new DeleteExhibitionQuestionRepository(_configuration);
        }

        public IReadRepository<ExhibitionQuestion> ReadRepository()
        {
            return new ReadExhibitionQuestionRepository(_configuration);
        }

        public IUpdateRepository<ExhibitionQuestion> UpdateRepository()
        {
            return new UpdateExhibitionQuestionRepository(_configuration);
        }
    }
}
