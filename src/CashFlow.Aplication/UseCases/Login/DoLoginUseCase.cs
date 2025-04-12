using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Exception.ExceptionsBase;
using ClashFlow.Domain.Repositories.Users;
using ClashFlow.Domain.Security.Cryptography;
using ClashFlow.Domain.Security.Tokens;

namespace CashFlow.Aplication.UseCases.Login
{
    public class DoLoginUseCase(
        IUsersReadOnlyRepository repository,
        IPasswordEncripter passwordEncripter,
        IAccessTokenGenerator accessTokenGenerator
    ) : IDoLoginUseCase
    {
        private readonly IUsersReadOnlyRepository _repository = repository;
        private readonly IPasswordEncripter _passwordEncripter = passwordEncripter;
        private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;

        public async Task<ResponseRegisteredUser> Execute(RequestLogin request)
        {
            var user = await _repository.GetUserByEmail(email: request.Email) ?? throw new InvalidLoginException();

            var passwordMatch = _passwordEncripter.Verify(
                password: request.Password,
                passwordHash: user.Password
            );

            if (passwordMatch == false)
            {
                throw new InvalidLoginException();
            }


            return new ResponseRegisteredUser
            {
                Name = user.Name,
                Token = _accessTokenGenerator.Generate(user),
            };

        }
    }
}
