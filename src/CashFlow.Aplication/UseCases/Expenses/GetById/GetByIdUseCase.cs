using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
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

            if (result is null)
            {
                throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
            }

            return _mapper.Map<ResponseExpense>(result);
        }
    }
}
