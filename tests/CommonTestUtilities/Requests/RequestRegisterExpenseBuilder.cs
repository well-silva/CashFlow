using Bogus;
using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests
{
    public class RequestRegisterExpenseBuilder
    {
        public static RequestRegisterExpenseDto Build()
        {
            var faker = new Faker();

            return new Faker<RequestRegisterExpenseDto>()
                .RuleFor(request => request.Title, faker.Commerce.Product())
                .RuleFor(request => request.Description, faker.Commerce.ProductDescription())
                .RuleFor(request => request.Amount, faker.Random.Decimal(1, 1000))
                .RuleFor(request => request.Date, faker.Date.Past())
                .RuleFor(request => request.PaymentType, faker.PickRandom<PaymentType>());
        }
    }
}
