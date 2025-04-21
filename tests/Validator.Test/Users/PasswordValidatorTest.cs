using CashFlow.Aplication.UseCases.Users;
using CashFlow.Communication.Requests;
using FluentValidation;
using Shouldly;

namespace Validator.Test.Users;
public class PasswordValidatorTest
{
    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    [InlineData("a")]
    [InlineData("aa")]
    [InlineData("aaa")]
    [InlineData("aaaa")]
    [InlineData("aaaaa")]
    [InlineData("aaaaaa")]
    [InlineData("aaaaaaa")]
    [InlineData("aaaaaaaa")]
    [InlineData("AAAAAAAA")]
    [InlineData("Aaaaaaaa")]
    [InlineData("Aaaaaaa1")]
    public void ErrorPasswordInvalid(string password)
    {
        // Arrange
        var validator = new PasswordValidator<RequestRegisterUser>();

        // Act
        var result = validator
            .IsValid(new ValidationContext<RequestRegisterUser>(new RequestRegisterUser()), password);

        // Assert
        result.ShouldBeFalse();
    }
}
