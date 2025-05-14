﻿using CashFlow.Domain.Extensions;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Reports;
using ClosedXML.Excel;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCases.Expenses.Reports.Excel
{
    class GenerateExpensesReportExcelUseCase : IGenerateExpensesReportExcelUseCase
    {
        private const string CURRENCY_SYMBOL = "R$";
        private readonly IExpensesReadOnlyRepository _repository;
        private readonly ILoggedUser _loggedUser;
        public GenerateExpensesReportExcelUseCase(IExpensesReadOnlyRepository repository, ILoggedUser loggedUser)
        {
            _repository = repository;
            _loggedUser = loggedUser;
        }
        public async Task<byte[]> Execute(DateOnly month)
        {
            var user = await _loggedUser.Get();
            var expenses = await _repository.FilterByMonth(user, month);

            if (expenses.Count == 0) return [];

            using var workbook = new XLWorkbook();

            workbook.Author = user.Name;
            workbook.Style.Font.FontSize = 12;
            workbook.Style.Font.FontName = "Times New Roman";

            var worksheet = workbook.Worksheets.Add(month.ToString("Y"));

            InsertHeader(worksheet);

            var raw = 2;
            foreach (var expense in expenses)
            {
                worksheet.Cell($"A{raw}").Value = expense.Title;
                worksheet.Cell($"B{raw}").Value = expense.Date;
                worksheet.Cell($"C{raw}").Value = expense.PaymentType.PaymentTypeToString();
                worksheet.Cell($"E{raw}").Value = expense.Description;
                worksheet.Cell($"D{raw}").Value = expense.Amount;
                worksheet.Cell($"D{raw}").Style.NumberFormat.Format = $"-{CURRENCY_SYMBOL} #,##0.00";
                raw++;
            }

            worksheet.Columns().AdjustToContents();

            var file = new MemoryStream();
            workbook.SaveAs(file);

            return file.ToArray();
        }

        private void InsertHeader(IXLWorksheet worksheet)
        {
            worksheet.Cell("A1").Value = ResourceReportGenerationMessage.TITLE;
            worksheet.Cell("B1").Value = ResourceReportGenerationMessage.DATE;
            worksheet.Cell("C1").Value = ResourceReportGenerationMessage.PAYMENT_TYPE;
            worksheet.Cell("D1").Value = ResourceReportGenerationMessage.AMOUNT;
            worksheet.Cell("E1").Value = ResourceReportGenerationMessage.DESCRIPTION;

            worksheet.Cells("A1:E1").Style.Font.Bold = true;
            worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#F5C2b6");

            worksheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell("B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell("C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            worksheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        }
    }
}
