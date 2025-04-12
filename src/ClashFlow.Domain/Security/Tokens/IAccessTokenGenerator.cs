using ClashFlow.Domain.Entities;

namespace ClashFlow.Domain.Security.Tokens
{
    public interface IAccessTokenGenerator
    {
        string Generate(User user);
    }
}
