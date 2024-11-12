using RagnarockTourGuide.Interfaces.CRUDFactoryInterfaces;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Services.CRUDServices;

namespace RagnarockTourGuide.Services.ConcreteFactories
{
    public class CRUDFactory<T> : ICRUDFactory<T>
    {
        public ICreateRepository<T> CreateRepository()
        {
            return new CreateRepository<T>();
        }

        public IReadRepository<T> ReadRepository()
        {
            return new ReadRepository<T>();
        }

        public IUpdateRepository<T> UpdateRepository()
        {
            return new UpdateRepository<T>();
        }
        public IDeleteRepository<T> DeleteRepository()
        {
            return new DeleteRepository<T>();
        }
    }
}
