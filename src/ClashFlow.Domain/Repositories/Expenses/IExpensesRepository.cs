using ClashFlow.Domain.Entities;

namespace ClashFlow.Domain.Repositories.Expenses
{
    public interface IExpensesRepository
    {
        Task Add(Expense expense);
        Task<List<Expense>> GetAll();
        Task<Expense?> GetById(long id);
    }
}
