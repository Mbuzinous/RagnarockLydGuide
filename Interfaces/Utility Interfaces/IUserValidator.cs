using RagnarockTourGuide.Enums;

namespace RagnarockTourGuide.Interfaces.PreviousRepos
{
    public interface IUserValidator
    {
        Role? ValidateUser(string email, string password);
        Role GetUserRole(ISession session);
    }
}
