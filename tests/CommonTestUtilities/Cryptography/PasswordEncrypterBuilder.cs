using CashFlow.Domain.Security.Cryptography;
using Moq;

namespace CommonTestUtilities.Cryptography;

public class PasswordEncrypterBuilder
{
    public static IPasswordEncrypter Build()
    {
        var mock = new Mock<IPasswordEncrypter>();
        mock.Setup(passwordEncrypter => passwordEncrypter.Encrypt(It.IsAny<string>())).Returns("kda!=sjksdfkljçfsljkfdljk");
        return mock.Object;
    }
}
