namespace Hyphenation.Library.Strategies
{
    using System;
    using System.Linq;
    using Enums;
    using Extensions;
    using Interfaces.Strategies;
    using Settings;

    /// <summary>
    /// Класс, реализующий алгоритм расстановки переносов в словах (алгоритм Христова в модификации Дымченко и Варсанофьева).
    /// </summary>
    internal sealed class ChristsStrategy : BaseStrategy,
        IHyphenationStrategy
    {
        public ChristsStrategy(LanguageSettings[] settingsArray) : base(settingsArray) { }

        #region IHyphenationStrategy Members

        /// <summary>
        /// Тип алгоритма для вставки переносов.
        /// </summary>
        public override AlgorithmType Type => AlgorithmType.Christs;

        #endregion IHyphenationStrategy Members

        #region Protected Members

        /// <summary>
        /// Расстановка переносов в исходном слове.
        /// </summary>
        /// <param name="sourceWord">Исходное слово.</param>
        /// <param name="hyphenSymbol">Символ переноса.</param>
        /// <param name="settings">Настройки языка с правилами/шаблонами переноса.</param>
        /// <returns>Целевое слово с расставленными переносами.</returns>
        protected override string PerformInsertHyphensInWord(string sourceWord, string hyphenSymbol, LanguageSettings settings)
        {
            var convertedWord = ConvertWordToCommonCharacters(sourceWord, settings);
            if (convertedWord.IsNullOrEmpty())
            {
                return sourceWord;
            }

            var targetWord = sourceWord;

            foreach (var rule in settings.Rules)
            {
                if (rule.Key == null || !convertedWord.Contains(rule.Key))
                {
                    continue;
                }

                var insertIndex = default(int);

                do
                {
                    insertIndex = convertedWord.IndexOf(rule.Key, StringComparison.Ordinal);
                    if (insertIndex == -1)
                    {
                        break;
                    }

                    insertIndex += rule.Value;
                    targetWord = targetWord.Insert(insertIndex, hyphenSymbol);
                    convertedWord = convertedWord.Insert(insertIndex, hyphenSymbol);
                } while (insertIndex != -1);
            }

            return targetWord;
        }

        #endregion Protected Members

        #region Private Members

        /// <summary>
        /// Преобразует исходное слово в общие символы (гласные, согласные, специальные символы).
        /// </summary>
        /// <param name="sourceWord">Исходное слово.</param>
        /// <param name="settings">Настройки языка с правилами переноса.</param>
        /// <returns>Преобразованное в общие символы исходное слово.</returns>
        private string ConvertWordToCommonCharacters(string sourceWord, LanguageSettings settings)
        {
            var convertedWord = sourceWord;

            foreach (var character in sourceWord
                .ToCharArray()
                .Distinct())
            {
                if (!char.IsLetter(character))
                {
                    continue;
                }

                var upperCharacter = char.ToUpper(character);

                if (settings.Vowels.Contains(upperCharacter))
                {
                    convertedWord = convertedWord.Replace(character, LanguageSettings.ConvertedVowelLetter);
                }
                else if (settings.Consonants.Contains(upperCharacter))
                {
                    convertedWord = convertedWord.Replace(character, LanguageSettings.ConvertedConsonantLetter);
                }
                else if (settings.Specials.Contains(upperCharacter))
                {
                    convertedWord = convertedWord.Replace(character, LanguageSettings.ConvertedSpecialLetter);
                }
            }

            return convertedWord;
        }

        #endregion Private Members
    }
}
