using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using ClashFlow.Domain.Repositories;
using ClashFlow.Domain.Repositories.Expenses;

namespace CashFlow.Aplication.UseCases.Expenses.Update
{
    class UpdateExpenseUseCase : IUpdateExpenseUseCase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExpenseUpdateOnlyRepository _repository;
        public UpdateExpenseUseCase(
            IMapper mapper, 
            IUnitOfWork unitOfWork, 
            IExpenseUpdateOnlyRepository repository
        ) 
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }
        public async Task Execute(RequestExpenseDto request, long id)
        {
            ValidateRequest(request);

            var expense = await _repository.GetById(id) ?? throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
            
            _mapper.Map(request, expense);

            _repository.Update(expense);

            await _unitOfWork.Commit();
        }

        private void ValidateRequest(RequestExpenseDto request)
        {
            var validator = new ExpenseValidator();

            var result = validator.Validate(request);

            if (result.IsValid == false)
            {
                var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
