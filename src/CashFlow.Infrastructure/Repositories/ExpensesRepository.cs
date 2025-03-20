using CashFlow.Infrastructure.DataAccess;
using ClashFlow.Domain.Entities;
using ClashFlow.Domain.Repositories.Expenses;

namespace CashFlow.Infrastructure.Repositories
{
    internal class ExpensesRepository : IExpensesRepository
    {
        private readonly CashFlowDbContext _dbContext;
        public ExpensesRepository(CashFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Add(Expense expense)
        {
            await _dbContext.Expenses.AddAsync(expense);
        }
        public async Task<List<Expense>> GetAll()
        public void Delete(Expense expense)
        {
            return await _dbContext.Expenses.AsNoTracking().ToListAsync();
        }

        public void GetById(int id)
        {
            return await _dbContext.Expenses.AsNoTracking().ToListAsync();
        }

        public void Update(Expense expense)
        {
            throw new NotImplementedException();
        }
    }
}
