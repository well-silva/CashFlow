using CashFlow.Communication.Responses;

namespace CashFlow.Aplication.UseCases.Expenses.GetById
{
    public interface IGetByIdUseCase
    {
        Task<ResponseExpense> Execute(long id);
    }
}
