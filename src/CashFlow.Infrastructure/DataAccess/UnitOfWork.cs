using ClashFlow.Domain.Repositories;

namespace CashFlow.Infrastructure.DataAccess
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly CashFlowDbContext _dbContext;
        public UnitOfWork(CashFlowDbContext dbContex)
        {
            _dbContext = dbContex;
        }
        public async Task Commit() => await _dbContext.SaveChangesAsync();
    }
}
