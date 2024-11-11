using Kojg_Ragnarock_Guide.Interfaces;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Interfaces
{
    public interface IUserCRUDRepository<T> : ICRUDRepository<T> where T : User
    {
        void Delete(int currentUserRole, int targetUserRole, int targetUserId);
    }
}
