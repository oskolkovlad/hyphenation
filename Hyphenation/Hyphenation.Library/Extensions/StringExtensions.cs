namespace Hyphenation.Library.Extensions
{
    using System.Diagnostics;

    internal static class StringExtensions
    {
        /// <summary>
        /// Указывает, действительно ли указанная строка является строкой <see langword="null"/> или пустой строкой ("").
        /// </summary>
        /// <param name="value">Строка для проверки.</param>
        /// <returns>Значение <see langword="true"/>, если строка равена <see langword="null"/> или пустой строке ("");
        /// в противном случае — значение <see langword="false"/>.</returns>
        [DebuggerHidden]
        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);
    }
}
