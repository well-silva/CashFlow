using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Aplication.UseCases.Users.Register;
public interface IRegisterUserUseCase
{
    Task<ResponseRegisteredUser> Execute(RequestRegisterUser request);
}