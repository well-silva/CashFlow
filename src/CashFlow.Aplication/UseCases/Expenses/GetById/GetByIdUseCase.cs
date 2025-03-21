using AutoMapper;
using CashFlow.Communication.Responses;
using ClashFlow.Domain.Repositories.Expenses;

namespace CashFlow.Aplication.UseCases.Expenses.GetById
{
    public class GetByIdUseCase : IGetByIdUseCase
    {
        private readonly IExpensesRepository _repository;
        private readonly IMapper _mapper;
        public GetByIdUseCase(IExpensesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ResponseExpense> Execute(long id)
        {
            var result = await _repository.GetById(id);

            return _mapper.Map<ResponseExpense>(result);
        }
    }
}
