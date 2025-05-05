﻿using CashFlow.Domain.Extensions;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Reports;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using System.Reflection;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Colors;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf
{
    class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
    {
        private const string CURRENCY_SYMBOL = "R$";
        private const int HEIGHT_ROW_EXPENSE_TABLE = 25;
        private readonly IExpensesReadOnlyRepository _repository;
        public GenerateExpensesReportPdfUseCase(IExpensesReadOnlyRepository repository)
        { 
            _repository = repository;

            GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
        }
        public async Task<byte[]> Execute(DateOnly month)
        {
            var expenses = await _repository.FilterByMonth(month);
            if (expenses.Count == 0) return [];

            var document = CreateDocument(month);
            var page = CreatePage(document);

            CreateHeaderWithProfileAndName(page);

            var totalAmountExpenses = expenses.Sum(expense => expense.Amount);
            CreateTotalSpentSection(page, month, totalAmountExpenses);
            
            expenses.ForEach(expense =>
            {
                var table = CreateExpenseTable(page);
                var row = table.AddRow();
                row.Height = HEIGHT_ROW_EXPENSE_TABLE;

                AddExpenseTItle(row.Cells[0], expense.Title);
                AddHeaderForAmount(row.Cells[3]);

                row = table.AddRow();
                row.Height = HEIGHT_ROW_EXPENSE_TABLE;

                row.Cells[0].AddParagraph(expense.Date.ToString("D"));
                SetStyleBaseForExpenseInformation(row.Cells[0]);
                row.Cells[0].Format.LeftIndent = 20;

                row.Cells[1].AddParagraph(expense.Date.ToString("t"));
                SetStyleBaseForExpenseInformation(row.Cells[1]);

                row.Cells[2].AddParagraph(expense.PaymentType.PaymentTypeToString());
                SetStyleBaseForExpenseInformation(row.Cells[2]);

                AddAmountForExpense(row.Cells[3], expense.Amount);

                if (string.IsNullOrWhiteSpace(expense.Description) == false)
                {
                    var description = table.AddRow();
                    description.Height = HEIGHT_ROW_EXPENSE_TABLE;

                    description.Cells[0].AddParagraph(expense.Description);
                    description.Cells[0].Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 10, Color = ColorsHelper.BLACK };
                    description.Cells[0].Shading.Color = ColorsHelper.GREEN_LIGHT;
                    description.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                    description.Cells[0].MergeRight = 2;
                    description.Cells[0].Format.LeftIndent = 20;

                    row.Cells[3].MergeDown = 1;
                }

                AddWhiteSpace(table);
            });

            return RenderDocument(document);
        }
        private Document CreateDocument(DateOnly month)
        {
            var document = new Document();
            document.Info.Title = $"{ResourceReportGenerationMessage.EXPENSES_FOR} {month:Y}";
            document.Info.Author = "CashFlow";

            var style = document.Styles["Normal"];
            style!.Font.Name = FontHelper.RALEWAY_REGULAR;

            return document;
        }
        private Section CreatePage(Document document)
        {
            var section = document.AddSection();
            section.PageSetup = document.DefaultPageSetup.Clone();

            section.PageSetup.PageFormat = PageFormat.A4;
            section.PageSetup.LeftMargin = 40;
            section.PageSetup.RightMargin = 40;
            section.PageSetup.TopMargin = 80;
            section.PageSetup.BottomMargin = 80;

            return section;
        }
        private byte[] RenderDocument(Document document)
        {
            var renderer = new PdfDocumentRenderer
            {
                Document = document
            };

            renderer.RenderDocument();

            using var file = new MemoryStream();
            renderer.PdfDocument.Save(file);

            return file.ToArray();
        }
        private void CreateHeaderWithProfileAndName(Section page)
        {
            var table = page.AddTable();

            table.AddColumn();
            table.AddColumn("300");

            var row = table.AddRow();

            var assembly = Assembly.GetExecutingAssembly();
            var directoryName = Path.GetDirectoryName(assembly.Location);
            var pathProfilePhoto = Path.Combine(directoryName!, "UseCases", "Expenses", "Reports", "Pdf", "Logo", "ProfilePhoto.jpg");

            row.Cells[0].AddImage(pathProfilePhoto);
            row.Cells[1].AddParagraph("Hey, Wellington Silva");
            row.Cells[1].Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 16 };
            row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
        }
        private Table CreateExpenseTable(Section page)
        {
            var table = page.AddTable();

            table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
            table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
            table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
            table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;

            return table;
        }
        private void AddExpenseTItle(Cell cell, string expenseTitle)
        {
            cell.AddParagraph(expenseTitle);
            cell.Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 14, Color = ColorsHelper.BLACK };
            cell.Shading.Color = ColorsHelper.RED_LIGHT;
            cell.VerticalAlignment = VerticalAlignment.Center;
            cell.MergeRight = 2;
            cell.Format.LeftIndent = 20;
        }
        private void AddHeaderForAmount(Cell cell)
        {
            cell.AddParagraph(ResourceReportGenerationMessage.AMOUNT);
            cell.Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 14, Color = ColorsHelper.WHITE };
            cell.Shading.Color = ColorsHelper.RED_DARK;
            cell.VerticalAlignment = VerticalAlignment.Center;
        }
        private void SetStyleBaseForExpenseInformation(Cell cell)
        {
            cell.Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 12, Color = ColorsHelper.BLACK };
            cell.Shading.Color = ColorsHelper.GREEN_DARK;
            cell.VerticalAlignment = VerticalAlignment.Center;
        }
        private void AddAmountForExpense(Cell cell, decimal amount)
        {
            cell.AddParagraph($"-{amount} {CURRENCY_SYMBOL}");
            cell.Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 14, Color = ColorsHelper.BLACK };
            cell.Shading.Color = ColorsHelper.WHITE;
            cell.VerticalAlignment = VerticalAlignment.Center;
        }
        private void AddWhiteSpace(Table table)
        {
            var row = table.AddRow();
            row.Height = 30;
            row.Borders.Visible = false;
        }
        private void CreateTotalSpentSection(Section page, DateOnly month, decimal totalAmountExpenses)
        {
            var paragraph = page.AddParagraph();
            paragraph.Format.SpaceBefore = 40;
            paragraph.Format.SpaceAfter = 40;
            var title = string.Format(ResourceReportGenerationMessage.TOTAL_SPENT_IN, month.ToString("Y"));

            paragraph.AddFormattedText(title, new Font { Name = FontHelper.RALEWAY_REGULAR, Size = 15 });
            paragraph.AddLineBreak();

            paragraph.AddFormattedText($"{totalAmountExpenses} {CURRENCY_SYMBOL}", new Font { Name = FontHelper.WORKSANS_BLACK, Size = 50 });
        }
    }
}
