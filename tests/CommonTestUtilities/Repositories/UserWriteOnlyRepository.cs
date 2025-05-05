using CashFlow.Domain.Repositories.Users;
using Moq;

namespace CommonTestUtilities.Repositories;
public class UserWriteOnlyRepository
{
    public static IUsersWriteOnlyRepository Build()
    {
        var mock = new Mock<IUsersWriteOnlyRepository>();

        return mock.Object;
    }
}
