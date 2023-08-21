namespace Hyphenation.Library.Interfaces.Strategies
{
    using Enums;

    internal interface IHyphenationStrategy
    {
        /// <summary>
        /// Тип алгоритма для вставки переносов.
        /// </summary>
        AlgorithmType Type { get; }

        /// <summary>
        /// Расстановка переносов в исходном тексте.
        /// </summary>
        /// <param name="sourceText">Исходный текст.</param>
        /// <param name="hyphenSymbol">Символ переноса.</param>
        /// <returns>Целевой текст с расставленными переносами.</returns>
        string InsertHyphens(string sourceText, string hyphenSymbol = null);
    }
}
