using Calculator;
using Xunit;

namespace Calculator.Tests;

/// <summary>
/// Тесты валидации пароля.
/// </summary>
public class PasswordValidatorTests
{
    [Theory]
    [InlineData("Password1")]   // 9 символов, заглавная, цифра
    [InlineData("Abcdefg1")]    // 8 символов
    [InlineData("LongPass123")]
    public void IsValid_MeetsRules_ReturnsTrue(string password)
    {
        bool result = PasswordValidator.IsValid(password);

        Assert.True(result);
    }

    [Theory]
    [InlineData("short")]              // слишком короткий
    [InlineData("NoDigitsHere")]       // нет цифр
    [InlineData("alllowercase123")]    // нет заглавной
    [InlineData("12345678")]           // нет букв
    public void IsValid_ViolatesRules_ReturnsFalse(string password)
    {
        bool result = PasswordValidator.IsValid(password);

        Assert.False(result);
    }

    [Fact]
    public void IsValid_Null_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => PasswordValidator.IsValid(null!));
    }

    [Fact]
    public void IsValid_EmptyString_ReturnsFalse()
    {
        Assert.False(PasswordValidator.IsValid(string.Empty));
    }
}
