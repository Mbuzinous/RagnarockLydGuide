namespace RagnarockTourGuide.Interfaces.FactoryInterfaces
{
    public interface IUpdateRepository<T>
    {
        Task UpdateAsync(T toBeUpdatedT, T oldT);
    }
}
