﻿using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Register
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
        public async Task<ResponseRegisteredExpense> Execute(RequestExpenseDto request)
        {
            ValidateRequest(request);

            var expense = _mapper.Map<Expense>(request);

            await _repository.Add(expense);

            await _unitOfWork.Commit();

            return _mapper.Map<ResponseRegisteredExpense>(expense);
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
