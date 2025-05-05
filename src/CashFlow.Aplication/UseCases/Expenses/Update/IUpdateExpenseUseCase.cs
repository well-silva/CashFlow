using CashFlow.Communication.Requests;

namespace CashFlow.Application.UseCases.Expenses.Update
{
    public interface IUpdateExpenseUseCase
    {
        Task Execute(RequestExpenseDto request, long id);
    }
}
