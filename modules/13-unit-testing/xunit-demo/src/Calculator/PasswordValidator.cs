namespace Calculator;

/// <summary>
/// Валидация паролей по общим правилам надёжности.
/// </summary>
public static class PasswordValidator
{
    /// <summary>
    /// Проверяет, что пароль соответствует требованиям:
    /// длина не менее 8 символов, хотя бы одна заглавная буква,
    /// хотя бы одна цифра.
    /// </summary>
    /// <param name="password">Проверяемый пароль.</param>
    /// <returns><c>true</c>, если пароль допустим; иначе <c>false</c>.</returns>
    /// <exception cref="ArgumentException">
    /// Бросается, если <paramref name="password"/> равен <c>null</c>.
    /// </exception>
    public static bool IsValid(string? password)
    {
        if (password is null)
        {
            throw new ArgumentException("Пароль не может быть null.", nameof(password));
        }

        return password.Length >= 8
            && password.Any(char.IsUpper)
            && password.Any(char.IsDigit);
    }
}
