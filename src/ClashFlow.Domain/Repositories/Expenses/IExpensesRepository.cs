using ClashFlow.Domain.Entities;

namespace ClashFlow.Domain.Repositories.Expenses
{
    public interface IExpensesRepository
    {
        Task Add(Expense expense);
        void Update(Expense expense);
        void Delete(Expense expense);
        void GetById(int id);
    }
}
