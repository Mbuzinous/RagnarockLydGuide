using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Interfaces
{
    public interface ICRUDRepository<T>
    {
        //Create
        Task CreateAsync(T toBeCreatedT);

        //Read
        Task<T> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        List<T> FilterListByNumber(List<T> listOfT, int filterNr);
        Task<List<int>> GetUsedNumbersAsync();

        //Update
        Task UpdateAsync(T toBeUpdatedT);

        //Delete
        Task DeleteAsync(DeleteParameter parameter);

    }
}
