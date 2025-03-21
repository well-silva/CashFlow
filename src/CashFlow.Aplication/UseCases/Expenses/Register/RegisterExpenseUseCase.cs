using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Exception.ExceptionsBase;
using ClashFlow.Domain.Entities;
using ClashFlow.Domain.Repositories;
using ClashFlow.Domain.Repositories.Expenses;

namespace CashFlow.Aplication.UseCases.Expenses.Register
{
    public class RegisterExpenseUseCase : IRegisterExpenseUseCase
    {
        private readonly IExpensesWriteOnlyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RegisterExpenseUseCase(
            IExpensesWriteOnlyRepository repository,
            IUnitOfWork unitOfWork,
            IMapper mapper
        )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseRegisteredExpense> Execute(RequestRegisterExpenseDto request)
        {
            ValidateRequest(request);

            var expense = _mapper.Map<Expense>(request);

            await _repository.Add(expense);

            await _unitOfWork.Commit();

            return _mapper.Map<ResponseRegisteredExpense>(expense);
        }

        private void ValidateRequest(RequestRegisterExpenseDto request)
        {
            var validator = new RegisterExpenseValidator();
            var result = validator.Validate(request);

            if (result.IsValid == false)
            {
                var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
