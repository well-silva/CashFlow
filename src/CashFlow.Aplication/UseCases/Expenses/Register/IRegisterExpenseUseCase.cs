using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Aplication.UseCases.Expenses.Register
{
    public interface IRegisterExpenseUseCase
    {
        Task<ResponseRegisteredExpense> Execute(RequestExpenseDto request);
    }
}
