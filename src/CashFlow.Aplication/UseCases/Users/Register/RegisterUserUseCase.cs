using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using ClashFlow.Domain.Repositories;
using ClashFlow.Domain.Repositories.Users;
using ClashFlow.Domain.Security.Cryptography;
using ClashFlow.Domain.Security.Tokens;
using FluentValidation.Results;

namespace CashFlow.Aplication.UseCases.Users.Register;
public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IMapper _mapper;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IUsersReadOnlyRepository _usersReadOnlyRepository;
    private readonly IUsersWriteOnlyRepository _usersWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public RegisterUserUseCase(
        IMapper mapper,
        IPasswordEncripter passwordEncripter,
        IUsersReadOnlyRepository userReadOnlyRepository,
        IUsersWriteOnlyRepository usersReadOnlyRepository,
        IUnitOfWork unitOfWork,
        IAccessTokenGenerator accessTokenGenerator
    )
    {
        _mapper = mapper;
        _passwordEncripter = passwordEncripter;
        _usersReadOnlyRepository = userReadOnlyRepository;
        _usersWriteOnlyRepository = usersReadOnlyRepository;
        _unitOfWork = unitOfWork;
        _accessTokenGenerator = accessTokenGenerator;
    }

    public async Task<ResponseRegisteredUser> Execute(RequestRegisterUser request)
    {
        await Validate(request);

        var user = _mapper.Map<ClashFlow.Domain.Entities.User>(request);
        user.Password = _passwordEncripter.Encrypt(request.Password);
        user.UserIdentifier = Guid.NewGuid();

        await _usersWriteOnlyRepository.Add(user);
        await _unitOfWork.Commit();

        return new ResponseRegisteredUser
        {
            Name = user.Name,
            Token = _accessTokenGenerator.Generate(user),
        };
    }

    private async Task Validate(RequestRegisterUser request)
    {
        var result = new RegisterUserValidator().Validate(request);

        var emailExist = await _usersReadOnlyRepository.ExistActiveUserWithEmail(request.Email);

        if (emailExist)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
        }

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}