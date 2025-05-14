﻿using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCases.Expenses.GetAll
{
    class GetAllExpensesUseCase : IGetAllExpensesUseCase
    {
        private readonly IExpensesReadOnlyRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;

        public GetAllExpensesUseCase(
            IExpensesReadOnlyRepository repository,
            IMapper mapper,
            ILoggedUser loggedUser
        )
        {
            _repository = repository;
            _mapper = mapper;
            _loggedUser = loggedUser;
        }
        public async Task<ResponseExpenses> Execute()
        {
            var loggedUser = await _loggedUser.Get();

            var result = await _repository.GetAll(loggedUser);

            return new ResponseExpenses
            {
                Expenses = _mapper.Map<List<ResponseShortExpense>>(result)
            };
        }
    }
}
