namespace Hyphenation.Library.Settings
{
    using System.Collections.Generic;
    using Enums;

    /// <summary>
    /// Класс, отображающий настройки языка для расстановки переносов в словах.
    /// </summary>
    internal abstract class LanguageSettings
    {
        public const char ConvertedConsonantLetter = 'c';
        public const char ConvertedVowelLetter = 'v';
        public const char ConvertedSpecialLetter = 'x';

        /// <summary>
        /// Минимальное количество символов в слове.
        /// Если в слове символов меньше заданного порога, то они не будут рассматриваться для расстановки переносов.
        /// </summary>
        public abstract int MinLettersCount { get; }

        /// <summary>
        /// Минимальное количество символов в левой части слова после переноса (та часть слова, которая остается на строке).
        /// </summary>
        public abstract int MinLeftHyphenLettersCount { get; }

        /// <summary>
        /// Минимальное количество символов в правой части слова после переноса (та часть слова, которая переносится на следующую строку).
        /// </summary>
        public abstract int MinRightHyphenLettersCount { get; }

        /// <summary>
        /// Файл со словами исключениями из TeX.
        /// </summary>
        public abstract string ExceptionsFile { get; }

        /// <summary>
        /// Файл ресурса с шаблонами из TeX.
        /// </summary>
        public abstract string PatternsFile { get; }

        /// <summary>
        /// Язык.
        /// </summary>
        public abstract Language Language { get; }

        /// <summary>
        /// Коллекция согласных букв.
        /// </summary>
        public abstract IReadOnlyCollection<char> Consonants { get; }

        /// <summary>
        /// Коллекция гласных букв.
        /// </summary>
        public abstract IReadOnlyCollection<char> Vowels { get; }

        /// <summary>
        /// Коллекция специальных букв.
        /// </summary>
        public abstract IReadOnlyCollection<char> Specials { get; }

        /// <summary>
        /// Словарь правил, где
        /// <c>key</c> - последовательность гласных, согласных и специальных сиволов;
        /// <c>value</c> - начальная позиция для вставки символа переноса.
        /// </summary>
        public abstract IReadOnlyDictionary<string, byte> Rules { get; }
    }
}
