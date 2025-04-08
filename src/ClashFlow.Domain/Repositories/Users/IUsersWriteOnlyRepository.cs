using ClashFlow.Domain.Entities;

namespace ClashFlow.Domain.Repositories.Users
{
    public interface IUsersWriteOnlyRepository
    {
        Task Add(User user);
    }
}
