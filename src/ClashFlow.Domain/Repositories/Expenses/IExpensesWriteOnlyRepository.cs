using ClashFlow.Domain.Entities;

namespace ClashFlow.Domain.Repositories.Expenses
{
    public interface IExpensesWriteOnlyRepository
    {
        Task Add(Expense expense);
        /// <summary>
        /// This function returns TRUE if the deletion was successul
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(long id);
    }
}
