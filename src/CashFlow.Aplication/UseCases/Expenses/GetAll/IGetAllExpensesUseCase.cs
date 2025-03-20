using CashFlow.Communication.Responses;

namespace CashFlow.Aplication.UseCases.Expenses.GetAll
{
    public interface IGetAllExpensesUseCase
    {
        Task<ResponseExpenses> Execute();
    }
}
