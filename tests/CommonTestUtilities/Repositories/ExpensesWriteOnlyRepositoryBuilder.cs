using CashFlow.Domain.Repositories;
using Moq;

namespace CommonTestUtilities.Repositories;

public class ExpensesWriteOnlyRepositoryBuilder
{
    public static IExpensesWriteOnlyRepository Build()
    {
        var mock = new Mock<IExpensesWriteOnlyRepository>();
        return mock.Object;
    }
}
