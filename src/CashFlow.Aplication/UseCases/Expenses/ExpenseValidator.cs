using CashFlow.Communication.Requests;
using CashFlow.Exception;
using FluentValidation;

namespace CashFlow.Aplication.UseCases.Expenses
{
    public class ExpenseValidator : AbstractValidator<RequestExpenseDto>
    {
        public ExpenseValidator()
        {
            RuleFor(expense => expense.Title)
                .NotEmpty()
                .WithMessage(ResourceErrorMessages.TITLE_REQUIRED);

            RuleFor(expense => expense.Amount)
                .NotNull()
                .GreaterThan(0)
                .WithMessage(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO);

            RuleFor(expense => expense.Date)
                .NotNull()
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage(ResourceErrorMessages.EXPENSES_CANNOT_FOR_THE_FUTURE);

            RuleFor(expense => expense.PaymentType)
                .IsInEnum()
                .WithMessage(ResourceErrorMessages.PAYMENT_TYPE_INVALID);
        }
    }
}
