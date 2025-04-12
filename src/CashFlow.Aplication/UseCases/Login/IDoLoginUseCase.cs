using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Aplication.UseCases.Login
{
    public interface IDoLoginUseCase
    {
        Task<ResponseRegisteredUser> Execute(RequestLogin request);
    }
}
