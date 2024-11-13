using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Interfaces.FactoryInterfaces
{
    public interface IDeleteRepository<T>
    {
        void Delete(DeleteParameter parameter);
    }
}
