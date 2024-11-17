using RagnarockTourGuide.Interfaces;

namespace RagnarockTourGuide.BackendController
{
    public class BackendController<T>
    {
        public IUserValidator UserValidator { get; private set; }
        public ICRUDRepository<T> CRUDRepository { get; private set; }

        public BackendController(ICRUDRepository<T> crudRepository, IUserValidator userValidator)
        {
            CRUDRepository = crudRepository;
            UserValidator = userValidator;
        }
    }
}
