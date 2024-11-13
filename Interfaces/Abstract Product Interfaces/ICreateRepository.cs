namespace RagnarockTourGuide.Interfaces.FactoryInterfaces
{
    public interface ICreateRepository<T>
    {
        Task CreateAsync(T toBeCreatedT);
    }
}
