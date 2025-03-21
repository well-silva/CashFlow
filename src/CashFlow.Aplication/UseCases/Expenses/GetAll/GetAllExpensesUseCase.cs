using AutoMapper;
using CashFlow.Communication.Responses;
using ClashFlow.Domain.Repositories.Expenses;

namespace CashFlow.Aplication.UseCases.Expenses.GetAll
{
    class GetAllExpensesUseCase : IGetAllExpensesUseCase
    {
        private readonly IExpensesReadOnlyRepository _repository;
        private readonly IMapper _mapper;

        public GetAllExpensesUseCase(IExpensesReadOnlyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ResponseExpenses> Execute()
        {
            var result = await _repository.GetAll();

            return new ResponseExpenses
            {
                Expenses = _mapper.Map<List<ResponseShortExpense>>(result)
            };
        }
    }
}
