namespace ClashFlow.Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}
