using System.Threading.Tasks;

namespace RagnarockTourGuide.Interfaces.FactoryInterfaces
{
    public interface IReadRepository<T>
    {
        Task<T> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        List<T> FilterListByNumber(List<T> listOfT, int filterNr);
        Task<List<int>> GetUsedNumbersAsync();
    }
}
