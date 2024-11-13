namespace RagnarockTourGuide.Interfaces.FactoryInterfaces
{
    public interface IReadRepository<T>
    {
        T GetById(int id);
        List<T> GetAll();
        List<T> FilterListByNumber(List<T> listOfT, int filterNr);
        Task<List<int>> GetUsedNumbersAsync();
    }
}
