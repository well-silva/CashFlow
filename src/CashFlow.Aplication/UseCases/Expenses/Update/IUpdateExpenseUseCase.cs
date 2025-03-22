using CashFlow.Communication.Requests;

namespace CashFlow.Aplication.UseCases.Expenses.Update
{
    public interface IUpdateExpenseUseCase
    {
        Task Execute(RequestExpenseDto request, long id);
    }
}
