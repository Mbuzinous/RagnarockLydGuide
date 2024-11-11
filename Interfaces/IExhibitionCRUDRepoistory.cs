using Kojg_Ragnarock_Guide.Interfaces;
using Kojg_Ragnarock_Guide.Models;

namespace RagnarockTourGuide.Interfaces
{
    public interface IExhibitionCRUDRepoistory<T> : ICRUDRepository<T> where T : Exhibition
    {
        void Delete(int id);
        List<T> FilterListByNumber(List<T> listofT, int filterNumber);
        Task<List<int>> GetUsedNumbersAsync();
    }
}
