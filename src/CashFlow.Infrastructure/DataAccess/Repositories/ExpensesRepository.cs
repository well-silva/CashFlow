﻿using ClashFlow.Domain.Entities;
using ClashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories
{
    internal class ExpensesRepository : IExpensesReadOnlyRepository, IExpensesWriteOnlyRepository, IExpenseUpdateOnlyRepository
    {
        private readonly CashFlowDbContext _dbContext;
        public ExpensesRepository(CashFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Add(Expense expense) => await _dbContext.Expenses.AddAsync(expense);
        public async Task<bool> Delete(long id)
        {
            var result = await _dbContext.Expenses.FirstOrDefaultAsync(expense => expense.Id == id);

            if (result is null) return false;

            _dbContext.Expenses.Remove(result);

            return true;
        }
        public async Task<List<Expense>> GetAll() => await _dbContext.Expenses.AsNoTracking().ToListAsync();
        async Task<Expense?> IExpensesReadOnlyRepository.GetById(long id) => await _dbContext.Expenses.AsNoTracking().FirstOrDefaultAsync(expense => expense.Id == id);
        async Task<Expense?> IExpenseUpdateOnlyRepository.GetById(long id) => await _dbContext.Expenses.FirstOrDefaultAsync(expense => expense.Id == id);
        public void Update(Expense expense) => _dbContext.Expenses.Update(expense);
        public async Task<List<Expense>> FilterByMonth(DateOnly date)
        {
            var startDate = new DateTime(year: date.Year, month: date.Month, day: 1).Date;

            var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);

            var endDate = new DateTime(
                                year: date.Year, 
                                month: date.Month, 
                                day: daysInMonth, 
                                hour: 23, 
                                minute: 59,
                                second: 59
                               );

            return await _dbContext
                .Expenses
                .AsNoTracking()
                .Where(expense => expense.Date >= startDate && expense.Date <= endDate)
                .OrderBy(expense => expense.Date)
                .ToListAsync();
        }
    }
}
