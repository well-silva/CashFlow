using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Expenses;
using CommonTestUtilities.Requests;
using Shouldly;

namespace UseCases.Test.Expenses.Register;
public class RegisterExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestRegisterExpenseBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Title.ShouldBe(request.Title);
    }

    [Fact]
    public async Task ErrorTitleEmpty()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestRegisterExpenseBuilder.Build();
        request.Title = string.Empty;

        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

        result.GetErros().ShouldHaveSingleItem().Contains(ResourceErrorMessages.TITLE_REQUIRED);
    }

    private RegisterExpenseUseCase CreateUseCase(User user)
    {
        var repository = ExpensesWriteOnlyRepositoryBuilder.Build();
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new RegisterExpenseUseCase(
            repository,
            unitOfWork,
            mapper,
            loggedUser
        );
    }
}
