using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Interfaces.FactoryInterfaces
{
    public interface IDeleteRepository<T>
    {
        Task DeleteAsync(DeleteParameter parameter);
    }
}
