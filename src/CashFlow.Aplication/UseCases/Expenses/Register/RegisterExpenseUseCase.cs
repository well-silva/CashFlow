using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Exception.ExceptionsBase;
using ClashFlow.Domain.Entities;
using ClashFlow.Domain.Enums;
using ClashFlow.Domain.Repositories;
using ClashFlow.Domain.Repositories.Expenses;

namespace CashFlow.Aplication.UseCases.Expenses.Register
{
    public class RegisterExpenseUseCase : IRegisterExpenseUseCase
    {
        private readonly IExpensesRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public RegisterExpenseUseCase(IExpensesRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseRegisteredExpense> Execute(RequestRegisterExpenseDto request)
        {
            ValidateRequest(request);

            var expense = new Expense
            {
                Title = request.Title,
                Description = request.Description,
                Date = request.Date,
                Amount = request.Amount,
                PaymentType = (PaymentType)request.PaymentType
            };

            await _repository.Add(expense);

            await _unitOfWork.Commit();

            return new ResponseRegisteredExpense();
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
