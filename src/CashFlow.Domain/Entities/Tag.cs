namespace CashFlow.Domain.Entities;

public class Tag
{
    public long Id { get; set; }
    public Enums.Tag Title { get; set; }
    public long ExpenseId { get; set; }
    public Expense Expense { get; set; } = default!;
}
