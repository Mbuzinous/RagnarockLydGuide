using Kojg_Ragnarock_Guide.Models;

namespace Kojg_Ragnarock_Guide.Interfaces
{
    public interface ICRUDRepository<T>
    {

        Task Create(T toBeCreatedT);
        T GetById(int id);
        List<T> GetAll();
        Task Update(T toBeUpdatedT, T oldT);
        void Delete(int id);

        List<T> FilterListByNumber(List<T> listofT, int filterNumber);
    }
}
