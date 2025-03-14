﻿using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories;

public interface IExpensesWriteOnlyRepository
{
    Task Add(Expense expense);
    /// <summary>
    /// This function returns true if the deletion was successful
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task Delete(long id);
}
