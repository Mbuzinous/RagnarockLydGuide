using RagnarockTourGuide.Models.Enums;

namespace RagnarockTourGuide.Interfaces
{
    public interface IUserValidator
    {
        Role? ValidateUser(string email, string password);
        Role GetUserRole(ISession session);
    }
}
