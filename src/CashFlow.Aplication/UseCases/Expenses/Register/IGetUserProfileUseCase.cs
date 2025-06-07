using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Expenses.Register;
public interface IGetUserProfileUseCase
{
    Task<ResponseUserProfile> Execute();
}
