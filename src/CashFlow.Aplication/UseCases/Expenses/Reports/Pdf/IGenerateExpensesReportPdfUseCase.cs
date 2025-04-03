using ClashFlow.Domain.Repositories.Expenses;

namespace CashFlow.Aplication.UseCases.Expenses.Reports.Pdf
{
    public interface IGenerateExpensesReportPdfUseCase
    {
        Task<byte[]> Execute(DateOnly month);
    }
}
