﻿using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses;
public interface IExpensesReadOnlyRepository
{
    Task<List<Expense>> GetAll(User user);
    Task<Expense?> GetById(User user, long id);
    Task<List<Expense>> FilterByMonth(User user, DateOnly date);
}
