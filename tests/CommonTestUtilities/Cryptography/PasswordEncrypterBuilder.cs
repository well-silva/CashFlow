using CashFlow.Domain.Security.Cryptography;
using Moq;

namespace CommonTestUtilities.Cryptography;
public class PasswordEncrypterBuilder
{
    private readonly Mock<IPasswordEncripter> _passwordEncripterMock;

    public PasswordEncrypterBuilder()
    {
        _passwordEncripterMock = new Mock<IPasswordEncripter>();

        _passwordEncripterMock
            .Setup(passwordEncripter => passwordEncripter.Encrypt(It.IsAny<string>()))
            .Returns("!%kdjkhdjkh");
    }

    public PasswordEncrypterBuilder Verify(string? password)
    {
        if (string.IsNullOrWhiteSpace(password) == false)
        {
            _passwordEncripterMock
                .Setup(passwordEncrypter => passwordEncrypter.Verify(password, It.IsAny<string>()))
                .Returns(true);
        }


        return this;
    }
    public IPasswordEncripter Build() => _passwordEncripterMock.Object;
}
