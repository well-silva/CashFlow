using ClashFlow.Domain.Entities;

namespace ClashFlow.Domain.Repositories.Users
{
    public interface IUsersReadOnlyRepository
    {
        Task<bool> ExistActiveUserWithEmail(string email);
        Task<User?> GetUserByEmail(string email);
    }
}
