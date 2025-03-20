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

        public void Delete(Expense expense)
        {
            throw new NotImplementedException();
        }

        public void GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Expense expense)
        {
            throw new NotImplementedException();
        }
    }
}
