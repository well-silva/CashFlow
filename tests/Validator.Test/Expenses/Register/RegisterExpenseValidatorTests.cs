using CashFlow.Application.UseCases.Expenses;
using CashFlow.Communication.Enums;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using Shouldly;

namespace Validator.Test.Expenses.Register
{
    public class RegisterExpenseValidatorTests
    {
        [Fact]
        public void Success()
        {
            //Arrange
            var validator = new ExpenseValidator();
            var request = RequestRegisterExpenseBuilder.Build();

            //Act
            var result = validator.Validate(request);

            //Assert
            result.ShouldNotBeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData("     ")]
        [InlineData(null)]
        public void ErrorTitleEmpty(string title)
        {
            //Arrange
            var validator = new ExpenseValidator();
            var request = RequestRegisterExpenseBuilder.Build();
            request.Title = title;

            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(
                erro => erro.ShouldHaveSingleItem(),
                erro => erro[0].ErrorMessage.ShouldBe(ResourceErrorMessages.TITLE_REQUIRED)
            );
        }

        [Fact]
        public void ErrorDateFuture()
        {
            //Arrange
            var validator = new ExpenseValidator();
            var request = RequestRegisterExpenseBuilder.Build();
            request.Date = DateTime.UtcNow.AddDays(1);

            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(
                erro => erro.ShouldHaveSingleItem(),
                erro => erro[0].ErrorMessage.ShouldBe(ResourceErrorMessages.EXPENSES_CANNOT_FOR_THE_FUTURE)
            );
        }

        [Fact]
        public void ErrorPaymentTypeInvalid()
        {
            //Arrange
            var validator = new ExpenseValidator();
            var request = RequestRegisterExpenseBuilder.Build();
            request.PaymentType = (PaymentType)800;

            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(
                erro => erro.ShouldHaveSingleItem(),
                erro => erro[0].ErrorMessage.ShouldBe(ResourceErrorMessages.PAYMENT_TYPE_INVALID)
            );
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public void ErrorAmountInvalid(decimal amount)
        {
            //Arrange
            var validator = new ExpenseValidator();
            var request = RequestRegisterExpenseBuilder.Build();
            request.Amount = amount;

            //Act
            var result = validator.Validate(request);

            //Assert
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldSatisfyAllConditions(
                erro => erro.ShouldHaveSingleItem(),
                erro => erro[0].ErrorMessage.ShouldBe(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO)
            );
        }
    }
}