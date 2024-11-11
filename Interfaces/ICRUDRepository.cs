using Kojg_Ragnarock_Guide.Models;

namespace Kojg_Ragnarock_Guide.Interfaces
{
    public interface ICRUDRepository<T>
    {

        Task CreateAsync(T toBeCreatedT);
        T GetById(int id);
        List<T> GetAll();
        Task UpdateAsync(T newT, T oldT);



        /*Move Delete method to specific repo fx. (IQuizRepository)*/
        //void Delete(int id);      
    }
}
