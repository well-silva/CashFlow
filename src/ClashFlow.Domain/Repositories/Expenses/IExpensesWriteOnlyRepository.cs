using ClashFlow.Domain.Entities;

namespace ClashFlow.Domain.Repositories.Expenses
{
    public interface IExpensesWriteOnlyRepository
    {
        Task Add(Expense expense);
    }
}
