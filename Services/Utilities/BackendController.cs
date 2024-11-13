using RagnarockTourGuide.Interfaces.CRUDFactoryInterfaces;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Interfaces.PreviousRepos;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Services.Utilities
{
    public class BackendController<T>
    {
        private IUserValidator _userValidator;
        private ICreateRepository<T> _createRepository;
        private IReadRepository<T> _readRepository;
        private IUpdateRepository<T> _updateRepository;
        private IDeleteRepository<T> _deleteRepository;

        public BackendController(ICRUDFactory<T> factory, IUserValidator userValidator)
        {
            CreateRepository = factory.CreateRepository();
            ReadRepository = factory.ReadRepository();
            UpdateRepository = factory.UpdateRepository();
            DeleteRepository = factory.DeleteRepository();
            UserValidator = userValidator;
        }

        public IUserValidator UserValidator { get => _userValidator; set => _userValidator = value; }
        public ICreateRepository<T> CreateRepository { get => _createRepository; set => _createRepository = value; }
        public IReadRepository<T> ReadRepository { get => _readRepository; set => _readRepository = value; }
        public IUpdateRepository<T> UpdateRepository { get => _updateRepository; set => _updateRepository = value; }
        public IDeleteRepository<T> DeleteRepository { get => _deleteRepository; set => _deleteRepository = value; }
    }
}
