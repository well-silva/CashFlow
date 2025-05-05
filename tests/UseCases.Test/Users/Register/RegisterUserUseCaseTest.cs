using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;
using Shouldly;

namespace UseCases.Test.Users.Register;

public class RegisterUserUseCaseTest
{
    private static RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var writeRepository = UserWriteOnlyRepository.Build();
        var passwordEncripter = new PasswordEncrypterBuilder().Build();
        var tokenGenerator = JwtTokenGeneratorBuilder.Build();
        var readRepository = new UserReadOnlyRepositoryBuilder();

        if (string.IsNullOrWhiteSpace(email) == false) 
        { 
            readRepository.ExistActiveUserWithEmail(email);
        }

        return new RegisterUserUseCase(
            mapper,
            passwordEncripter,
            readRepository.Build(),
            writeRepository,
            unitOfWork,
            tokenGenerator
        );
    }

    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = RequestRegisterUserBuilder.Build();
        var useCase = CreateUseCase();

        // Act
        var result = await useCase.Execute(request);

        // Assert
        result.ShouldNotBeNull();
        result.Name.ShouldBe(request.Name);
        result.Token.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task ErrorNameEmpty()
    {
        // Arrange
        var request = RequestRegisterUserBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase();

        // Act
        var act = async () => await useCase.Execute(request);

        // Assert
        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();
        result.GetErros().ShouldHaveSingleItem().Contains(ResourceErrorMessages.NAME_EMPTY);
    }

    [Fact]
    public async Task ErrorEmailAlreadyExist()
    {
        // Arrange
        var request = RequestRegisterUserBuilder.Build();

        var useCase = CreateUseCase(request.Email);

        // Act
        var act = async () => await useCase.Execute(request);

        // Assert
        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();
        result.GetErros().ShouldHaveSingleItem().Contains(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED);
    }
}
