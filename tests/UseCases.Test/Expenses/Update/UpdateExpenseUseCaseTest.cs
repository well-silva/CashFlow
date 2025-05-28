using CashFlow.Application.UseCases.Expenses.Update;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Expenses;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Test.Expenses.Update;
public class UpdateExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestExpenseBuilder.Build();
        var expense = ExpenseBuilder.Build(loggedUser);

        var useCase = CreateUseCase(loggedUser, expense);

        var act = async () => await useCase.Execute(request, expense.Id);

        await act.Should().NotThrowAsync();

        expense.Title.Should().Be(request.Title);
        expense.Description.Should().Be(request.Description);
        expense.Amount.Should().Be(request.Amount);
        expense.Date.Should().Be(request.Date);
        expense.PaymentType.Should().Be((PaymentType)request.PaymentType);
    }

    [Fact]
    public async Task ErrorTitleEmpty()
    {
        var loggedUser = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(loggedUser);

        var request = RequestExpenseBuilder.Build();
        request.Title = string.Empty;

        var useCase = CreateUseCase(loggedUser, expense);

        var act = async () => await useCase.Execute(request, id: expense.Id);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex => ex.GetErros().Count == 1 && ex.GetErros().Contains(ResourceErrorMessages.TITLE_REQUIRED));
    }

    [Fact]
    public async Task ErrorExpenseNotFound()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestExpenseBuilder.Build();

        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(request, id: 1000);

        var result = await act.Should().ThrowAsync<NotFoundException>();

        result.Where(ex => ex.GetErros().Count == 1 && ex.GetErros().Contains(ResourceErrorMessages.EXPENSE_NOT_FOUND));

    }

    private UpdateExpenseUseCase CreateUseCase(User user, Expense? expense = null)
    {
        var repository = new ExpensesUpdateOnlyRepositoryBuilder()
            .GetById(user, expense)
            .Build();
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new UpdateExpenseUseCase(
            mapper,
            unitOfWork,
            repository,
            loggedUser
        );
    }
}
