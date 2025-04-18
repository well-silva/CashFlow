﻿using ClashFlow.Domain.Enums;
using ClashFlow.Domain.Extensions;
using ClashFlow.Domain.Reports;
using ClashFlow.Domain.Repositories.Expenses;
using ClosedXML.Excel;

namespace CashFlow.Aplication.UseCases.Expenses.Reports.Excel
{
    class GenerateExpensesReportExcelUseCase : IGenerateExpensesReportExcelUseCase
    {
        private const string CURRENCY_SYMBOL = "R$";
        private readonly IExpensesReadOnlyRepository _repository;
        public GenerateExpensesReportExcelUseCase(IExpensesReadOnlyRepository repository)
        {
            _repository = repository;
        }
        public async Task<byte[]> Execute(DateOnly month)
        {
            var expenses = await _repository.FilterByMonth(month);

            if (expenses.Count == 0) return [];

            using var workbook = new XLWorkbook();

            workbook.Author = "CashFlow";
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
