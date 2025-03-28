using CashFlow.Communication.Requests;

namespace CashFlow.Aplication.UseCases.Expenses.Reports.Excel
{
    public interface IGenerateExpensesReportExcelUseCase
    {
        Task<byte[]> Execute(DateOnly month);
    }
}
