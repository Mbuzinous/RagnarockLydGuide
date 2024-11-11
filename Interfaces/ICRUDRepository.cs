namespace Kojg_Ragnarock_Guide.Interfaces
{
    public interface ICRUDRepository<T>
    {

        Task CreateAsync(T toBeCreatedT);
        T GetById(int id);
        List<T> GetAll();
        Task UpdateAsync(T toBeUpdatedT, T oldT);
        
        
        
        
        /*Move Delete method to specific repo fx. (IQuizRepository)*/
        //void Delete(int id);      
    }
}
