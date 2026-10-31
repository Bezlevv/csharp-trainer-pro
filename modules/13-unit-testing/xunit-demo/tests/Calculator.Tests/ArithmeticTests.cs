using Calculator;
using Xunit;

namespace Calculator.Tests;

/// <summary>
/// Тесты арифметических операций.
/// </summary>
public class ArithmeticTests
{
    [Fact]
    public void Add_TwoNumbers_ReturnsSum()
    {
        // Arrange & Act
        decimal result = Arithmetic.Add(2.5m, 3.5m);

        // Assert
        Assert.Equal(6m, result);
    }

    [Fact]
    public void Subtract_SubtractsSecondFromFirst()
    {
        decimal result = Arithmetic.Subtract(10m, 4m);

        Assert.Equal(6m, result);
    }

    [Theory]
    [InlineData(2, 3, 6)]
    [InlineData(-2, 3, -6)]
    [InlineData(0, 5, 0)]
    public void Multiply_TwoNumbers_ReturnsProduct(decimal a, decimal b, decimal expected)
    {
        decimal result = Arithmetic.Multiply(a, b);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Divide_ByNonZero_ReturnsQuotient()
    {
        decimal result = Arithmetic.Divide(9m, 3m);

        Assert.Equal(3m, result);
    }

    [Fact]
    public void Divide_ByZero_ThrowsDivideByZeroException()
    {
        Assert.Throws<DivideByZeroException>(() => Arithmetic.Divide(5m, 0));
    }

    [Fact]
    public void Divide_PrecisionPreserved_DecimalIsNotRounded()
    {
        decimal result = Arithmetic.Divide(1m, 3m);

        Assert.Equal(1m / 3m, result);
    }
}
