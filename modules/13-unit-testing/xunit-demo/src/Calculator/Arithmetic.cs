namespace Calculator;

/// <summary>
/// Простые арифметические операции над десятичными числами.
/// </summary>
public static class Arithmetic
{
    /// <summary>Возвращает сумму двух чисел.</summary>
    public static decimal Add(decimal a, decimal b) => a + b;

    /// <summary>Возвращает разность двух чисел.</summary>
    public static decimal Subtract(decimal a, decimal b) => a - b;

    /// <summary>Возвращает произведение двух чисел.</summary>
    public static decimal Multiply(decimal a, decimal b) => a * b;

    /// <summary>
    /// Возвращает частное от деления.
    /// </summary>
    /// <exception cref="DivideByZeroException">
    /// Бросается, если <paramref name="b"/> равен нулю.
    /// </exception>
    public static decimal Divide(decimal a, decimal b)
    {
        if (b == 0)
        {
            throw new DivideByZeroException("Деление на ноль невозможно.");
        }

        return a / b;
    }
}
