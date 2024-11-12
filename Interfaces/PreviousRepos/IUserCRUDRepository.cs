using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Interfaces.PreviousRepos
{
    public interface IUserCRUDRepository<T> : ICRUDRepository<T> where T : User
    {
        void Delete(int currentUserRole, int targetUserRole, int targetUserId);
    }
}
