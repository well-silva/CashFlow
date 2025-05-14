using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using Shouldly;

namespace Validator.Test.Users.Register;
public class RegisterUserValidatorTest
{
    [Fact]
    public void Success()
    {
        // Arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserBuilder.Build();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void ErrorNameEmpty(string name)
    {
        // Arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserBuilder.Build();
        request.Name = name;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();
        var error = result.Errors.ShouldHaveSingleItem();
        error.ErrorMessage.ShouldBe(ResourceErrorMessages.NAME_EMPTY);
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void ErrorEmailEmpty(string email)
    {
        // Arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserBuilder.Build();
        request.Email = email;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceErrorMessages.EMAIL_EMPTY);
    }

    [Fact]
    public void ErrorEmailInvalid()
    {
        // Arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserBuilder.Build();
        request.Email = "teste.com";

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceErrorMessages.EMAIL_INVALID);
    }

    [Fact]
    public void ErrorPasswordEmpty()
    {
        // Arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserBuilder.Build();
        request.Password = string.Empty;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe(ResourceErrorMessages.INVALID_PASSWORD);
    }
}

