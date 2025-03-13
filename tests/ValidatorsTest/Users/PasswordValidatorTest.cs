using CashFlow.Application.UseCases.User;
using CashFlow.Communication.Requests;
using FluentAssertions;
using FluentValidation;

namespace ValidatorsTest.Users;
public class PasswordValidatorTest
{
    [Theory]
    [InlineData("")]
    [InlineData("        ")]
    [InlineData(null)]
    [InlineData("aaaaaaa")]
    [InlineData("aaaaaaaa")]
    [InlineData("AAAAAAAA")]
    [InlineData("Aaaaaaaa")]
    [InlineData("Aaaaaaa1")]
    public void Error_Password_Invalid(string password)
    {
        var validator = new PasswordValidator<RequestRegisterUserJson>();

        var result = validator.IsValid(new ValidationContext<RequestRegisterUserJson>(new RequestRegisterUserJson()), password);
        result.Should().BeFalse();
    }
}
