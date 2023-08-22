namespace Hyphenation.Library.Strategies
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Constants;
    using Enums;
    using Extensions;
    using Settings;

    internal abstract class BaseStrategy
    {
        private const int MinAbbreviationOrReductionUpperCharCount = 2;
        private const string InputPattern = @"^{0}(?=\W+)|(?<=\W+){0}(?=\W+)|(?<=\W+){0}$|^{0}$";

        private static readonly IDictionary<string, string> WordsWithHyphens = new Dictionary<string, string>();
        
        private readonly LanguageSettings[] _settingsArray;

        protected BaseStrategy(LanguageSettings[] settingsArray)
        {
            _settingsArray = settingsArray ?? new LanguageSettings[0];
        }

        #region Public Members

        /// <summary>
        /// Тип алгоритма для вставки переносов.
        /// </summary>
        public abstract AlgorithmType Type { get; }

        /// <summary>
        /// Расстановка переносов в исходном тексте.
        /// </summary>
        /// <param name="sourceText">Исходный текст.</param>
        /// <param name="hyphenSymbol">Символ переноса.</param>
        /// <returns>Целевой текст с расставленными переносами.</returns>
        public string InsertHyphens(string sourceText, string hyphenSymbol = null)
        {
            if (sourceText.IsNullOrEmpty())
            {
                return sourceText;
            }

            if (hyphenSymbol.IsNullOrEmpty())
            {
                hyphenSymbol = Symbols.SoftHyphenSymbol.ToString();
            }

            var targetText = sourceText;

            foreach (var sourceWord in GetSourceWords(targetText))
            {
                var targetWord = InsertHyphensInWord(sourceWord, hyphenSymbol);
                if (targetWord.IsNullOrEmpty())
                {
                    continue;
                }

                if (targetWord == sourceWord)
                {
                    continue;
                }

                var tempTargetText = ReplaceWordsInText(targetText, sourceWord, targetWord);
                targetText = tempTargetText ?? targetText;
            }

            return targetText;
        }

        #endregion IHyphenationInsertAlgorithmStrategy Members

        #region Protected Members

        /// <summary>
        /// Расстановка переносов в исходном слове.
        /// </summary>
        /// <param name="sourceWord">Исходное слово.</param>
        /// <param name="hyphenSymbol">Символ переноса.</param>
        /// <param name="settings">Настройки языка с правилами/шаблонами переноса.</param>
        /// <returns>Целевое слово с расставленными переносами.</returns>
        protected abstract string PerformInsertHyphensInWord(string sourceWord, string hyphenSymbol, LanguageSettings settings);

        #endregion Protected Members

        #region Private Members

        /// <summary>
        /// Поиск специальных символов, которые могут использоваться в словах.
        /// </summary>
        /// <param name="sourceWord">Исходное слово.</param>
        /// <param name="hyphenSymbol">Символ переноса.</param>
        /// <param name="targetWord">Целевое слово.</param>
        /// <returns>Результат поиска.</returns>
        private bool FindSpecialWordSymbols(string sourceWord, string hyphenSymbol, out string targetWord)
        {
            targetWord = sourceWord;

            var sourceCharacters = sourceWord.ToCharArray();
            if (!sourceCharacters.Intersect(Symbols.WordSymbols).Any())
            {
                return false;
            }

            if (!sourceCharacters.Contains(Symbols.HyphenSymbol))
            {
                return true;
            }

            var count = default(int);
            foreach (var character in sourceCharacters)
            {
                if (character == Symbols.HyphenSymbol)
                {
                    targetWord = targetWord.Insert(count, hyphenSymbol);
                    count++;
                }

                count++;
            }

            return true;
        }

        private LanguageSettings GetLanguageSettings(string sourceWord)
        {
            var settings = default(LanguageSettings);

            if (Regex.IsMatch(sourceWord, "[А-Яа-яЁё]+"))
            {
                settings = GetLanguageSettings(Language.Russian);
            }
            else if (Regex.IsMatch(sourceWord, "[A-Za-z]+"))
            {
                settings = GetLanguageSettings(Language.English);
            }

            return settings;
        }

        /// <summary>
        /// Получение настройки языка с правилами/шаблонами переноса.
        /// </summary>
        /// <param name="language">Язык.</param>
        /// <returns>Настройки языка с правилами/шаблонами переноса.</returns>
        private LanguageSettings GetLanguageSettings(Language language) =>
            _settingsArray.FirstOrDefault(settings => settings.Language == language);

        /// <summary>
        /// Возвращает исходные слова из текста.
        /// </summary>
        /// <param name="sourceText">Исходный текст.</param>
        /// <returns>Перечисление исходных слов из текста.</returns>
        private IEnumerable<string> GetSourceWords(string sourceText)
        {
            return !sourceText.IsNullOrEmpty() ?
                sourceText
                    .Split(Symbols.TextSeparators)
                    .Select(word => word.Trim())
                    .Where(word => !word.IsNullOrEmpty())
                    .Distinct() :
                Enumerable.Empty<string>();
        }

        /// <summary>
        /// Расстановка переносов в исходном слове.
        /// </summary>
        /// <param name="sourceWord">Исходное слово.</param>
        /// <param name="hyphenSymbol">Символ переноса.</param>
        /// <returns>Целевое слово с расставленными переносами.</returns>
        private string InsertHyphensInWord(string sourceWord, string hyphenSymbol)
        {
            var settings = GetLanguageSettings(sourceWord);
            if (settings == null)
            {
                return sourceWord;
            }

            var trimmedSourceWord = sourceWord.Trim(Symbols.TextSeparators).Trim();
            if (WordsWithHyphens.ContainsKey(trimmedSourceWord))
            {
                return WordsWithHyphens[trimmedSourceWord];
            }

            var sourceCharacters = trimmedSourceWord.ToCharArray();

            // Отсекаем слова по минимальному количеству символов в слове,
            // проверяем на наличие символа мягкого переноса,
            // игнорируем аббревиатуры и сокращения.
            if (trimmedSourceWord.Length < settings.MinLettersCount ||
                sourceCharacters.Contains(Symbols.SoftHyphenSymbol) ||
                IsAbbreviationOrReduction(sourceWord))
            {
                return sourceWord;
            }

            if (FindSpecialWordSymbols(trimmedSourceWord, hyphenSymbol, out var targetWord))
            {
                WordsWithHyphens.Add(trimmedSourceWord, targetWord);
                return targetWord;
            }

            try
            {
                targetWord = PerformInsertHyphensInWord(trimmedSourceWord, hyphenSymbol, settings);
            }
            catch
            {
                // TODO: Log.
                return sourceWord;
            }

            if (targetWord.IsNullOrEmpty())
            {
                WordsWithHyphens.Add(trimmedSourceWord, trimmedSourceWord);
                return trimmedSourceWord;
            }

            WordsWithHyphens.Add(trimmedSourceWord, targetWord);
            return targetWord;
        }

        /// <summary>
        /// Проверяет: является ли исходное слово аббревиатурой или сокращением.
        /// </summary>
        /// <param name="sourceWord">Исходное слово.</param>
        /// <returns>Результат проверки.</returns>
        private bool IsAbbreviationOrReduction(string sourceWord) =>
            !sourceWord.IsNullOrEmpty() && sourceWord.ToCharArray().Count(char.IsUpper) >= MinAbbreviationOrReductionUpperCharCount;

        /// <summary>
        /// Замена исходного слова в тексте на целовое слово с расставленными переносами.
        /// </summary>
        /// <param name="targetText">Целевой текст.</param>
        /// <param name="sourceWord">Исходное слово.</param>
        /// <param name="targetWord">Целевое слово.</param>
        /// <returns>Целевой текст.</returns>
        private string ReplaceWordsInText(string targetText, string sourceWord, string targetWord)
        {
            try
            {
                sourceWord = PrepareSourceWordForRegularExpression(sourceWord);
                var pattern = string.Format(InputPattern, sourceWord);
                targetText = Regex.Replace(targetText, pattern, targetWord);
            }
            catch
            {
                // TODO: Log.
            }

            return targetText;
        }

        /// <summary>
        /// Подготовка исходного слова для регулярного выражения.
        /// </summary>
        /// <param name="sourceWord">Исходное слово.</param>
        /// <returns>Отформатированное исходное слово.</returns>
        private string PrepareSourceWordForRegularExpression(string sourceWord)
        {
            foreach (var symbol in Symbols.EscapeSymbols.Keys)
            {
                if (sourceWord.Contains(symbol))
                {
                    sourceWord = sourceWord.Replace(symbol, Symbols.EscapeSymbols[symbol]);
                }
            }

            return sourceWord;
        }

        #endregion Private Members
    }
}
