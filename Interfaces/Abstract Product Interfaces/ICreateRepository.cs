using Kojg_Ragnarock_Guide.Models;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Interfaces.FactoryInterfaces
{
    public interface ICreateRepository<T>
    {
        void Create(Exhibition exhibition);
        void Create(User user);

    }
}
