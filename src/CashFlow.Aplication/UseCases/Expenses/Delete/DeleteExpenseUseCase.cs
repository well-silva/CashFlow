
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using ClashFlow.Domain.Repositories;
using ClashFlow.Domain.Repositories.Expenses;

namespace CashFlow.Aplication.UseCases.Expenses.Delete
{
    class DeleteExpenseUseCase : IDeleteExpenseUseCase
    {
        private readonly IExpensesWriteOnlyRepository _respository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteExpenseUseCase(IExpensesWriteOnlyRepository respository, IUnitOfWork unitOfWork)
        {
            _respository = respository;
            _unitOfWork = unitOfWork;
        }
        public async Task Execute(long id)
        {
            var result = await _respository.Delete(id);

            if (result == null)
            {
                throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
            }

            await _unitOfWork.Commit();
        }
    }
}
