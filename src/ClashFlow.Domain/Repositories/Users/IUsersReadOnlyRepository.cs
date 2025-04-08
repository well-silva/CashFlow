namespace ClashFlow.Domain.Repositories.Users
{
    public interface IUsersReadOnlyRepository
    {
        Task<bool> ExistActiveUserWithEmail(string email);
    }
}
