
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using ClashFlow.Domain.Repositories;
using ClashFlow.Domain.Repositories.Expenses;

namespace CashFlow.Aplication.UseCases.Expenses.Delete
{
    public class DeleteExpenseUseCase : IDeleteExpenseUseCase
    {
        private readonly IExpensesWriteOnlyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteExpenseUseCase(
            IExpensesWriteOnlyRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(long id)
        {
            var result = await _repository.Delete(id);

            if (result == false)
            {
                throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
            }

            await _unitOfWork.Commit();
        }
    }
}
