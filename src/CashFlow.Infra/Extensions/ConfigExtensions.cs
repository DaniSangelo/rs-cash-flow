using Microsoft.Extensions.Configuration;

namespace CashFlow.Infra.Extensions;

public static class ConfigExtensions
{
    public static bool IsTestEnv(this IConfiguration configuration)
    {
        return configuration.GetValue<bool>("InMemoryTest");
    }
}
