using RagnarockTourGuide.Interfaces.FactoryInterfaces;

namespace RagnarockTourGuide.Interfaces.CRUDFactoryInterfaces
{
    public interface ICRUDFactory<T>
    {
        ICreateRepository<T> CreateRepository();
        IReadRepository<T> ReadRepository();
        IUpdateRepository<T> UpdateRepository();
        IDeleteRepository<T>    DeleteRepository();
    }
}
