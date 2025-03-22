using ClashFlow.Domain.Entities;

namespace ClashFlow.Domain.Repositories.Expenses
{
    public interface IExpenseUpdateOnlyRepository
    {
        Task<Expense?> GetById(long id);
        void Update(Expense expense);
    }
}
