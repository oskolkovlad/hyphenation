namespace Hyphenation.Library.Strategies
{
    using System.Collections.Generic;
    using System.Linq;
    using Enums;
    using Extensions;
    using Interfaces.Strategies;
    using Settings;
    using Settings.Patterns;

    /// <summary>
    /// Класс, реализующий алгоритм расстановки переносов в словах (алгоритм Ляна-Кнута).
    /// 
    /// В шаблоне расстановки переносов в TeX есть три вида символов.
    /// Точка - является привязкой к границе слова.
    /// Буква - обозначает саму себя, то есть букву в слове, которое переносится через дефис.
    /// Число - обозначает потенциальную точку переноса, а число обозначает уровень переноса.
    /// 
    /// Основная идея алгоритма заключается в том, что слово сопоставляется с шаблонами и на их основе заполняется уровнями.
    /// Если два уровня из двух разных шаблонов совпадают в одной и той же точке, выбирается более высокий.
    /// Из конечных значений только нечетные уровни указывают на допустимые точки переноса. Идея состоит в том, чтобы иметь возможность указывать
    /// как возможные точки переноса, так и места, где дефис не следует вставлять. Так, например, если определенное место в слове соответствует
    /// двум шаблонам, в которых есть 1 и 2 в этом месте, перенос в этой точке запрещен, потому что 2 переопределяет 1, и только нечетное значение
    /// указывает на разрешенную точку расстановки переносов.
    /// </summary>
    internal sealed class LiangKnuthStrategy : BaseStrategy,
        IHyphenationStrategy
    {
        private static readonly IDictionary<Language, LanguagePatterns> _patterns = new Dictionary<Language, LanguagePatterns>();

        public LiangKnuthStrategy(LanguageSettings[] settingsArray) : base(settingsArray) { }

        #region IHyphenationStrategy Members

        /// <summary>
        /// Тип алгоритма для вставки переносов.
        /// </summary>
        public override AlgorithmType Type => AlgorithmType.LiangKnuth;

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
            var patterns = GetLanguagePatterns(settings);
            var exceptionPattern = patterns.Exceptions.FirstOrDefault(exception => exception.ClearWord == sourceWord);

            if (exceptionPattern != null)
            {
                exceptionPattern.ReplaceHyphenSymbol(hyphenSymbol);
                return exceptionPattern.Word;
            }

            var targetWord = FormatWord(sourceWord, true);
            var levels = new Dictionary<int, int>();

            for (var startIndex = 0; startIndex < targetWord.Length - 1; startIndex++)
            {
                var actualPatterns = new List<Pattern>();
                actualPatterns.AddRange(patterns.Patterns);

                for (var length = 1; length <= targetWord.Length - startIndex; length++)
                {
                    var subTargetWord = targetWord.Substring(startIndex, length);
                    var currentPattern = actualPatterns.FirstOrDefault(pattern => subTargetWord.StartsWith(pattern.StringRule));
                    if (currentPattern == null)
                    {
                        continue;
                    }

                    SetWordLevels(targetWord.Length, startIndex, settings, currentPattern, levels);
                    actualPatterns.Remove(currentPattern);
                }
            }

            targetWord = InsertHyphensInWord(sourceWord, hyphenSymbol, levels);
            return targetWord;
        }

        #endregion Protected Members

        #region Private Members

        /// <summary>
        /// Подготовка слова к поиску шаблонов.
        /// </summary>
        /// <param name="word">Cлово.</param>
        /// <param name="doLower">Флаг, отвечающий за приведение слова к нижнему регистру.</param>
        /// <returns>Отформатированное слово.</returns>
        private string FormatWord(string word, bool doLower) =>
            !word.IsNullOrEmpty() ? string.Format(".{0}.", doLower ? word.ToLower() : word) : null;

        /// <summary>
        /// Получение шаблонов и слов-исключений языка.
        /// </summary>
        /// <param name="settings">Настройки языка для расстановки переносов в словах.</param>
        /// <returns>Шаблоны и слова-исключений языка.</returns>
        private LanguagePatterns GetLanguagePatterns(LanguageSettings settings)
        {
            if (!_patterns.ContainsKey(settings.Language))
            {
                _patterns[settings.Language] = new LanguagePatterns(settings);
            }

            var patterns = _patterns[settings.Language];
            return patterns;
        }

        /// <summary>
        /// Расстановка переносов в исходном слове.
        /// </summary>
        /// <param name="sourceWord">Исходное слово.</param>
        /// <param name="hyphenSymbol">Символ переноса.</param>
        /// <param name="levels">Уровни (приоритеты) для расстановки переносов.</param>
        /// <returns>Целевое слово с расставленными переносами.</returns>
        private string InsertHyphensInWord(string sourceWord, string hyphenSymbol, IReadOnlyDictionary<int, int> levels)
        {
            if (levels == null || levels.Count == 0)
            {
                return null;
            }

            var offset = 0;
            var targetWord = FormatWord(sourceWord, false);

            foreach (var level in levels.OrderBy(pair => pair.Key))
            {
                if (level.Value % 2 == 0)
                {
                    continue;
                }

                targetWord = targetWord.Insert(level.Key + offset, hyphenSymbol);
                offset++;
            }

            return targetWord.Trim('.', '-');
        }

        /// <summary>
        /// Установка уровней (приоритетов) для слова.
        /// </summary>
        /// <param name="wordLength">Количество символов в исходной слове.</param>
        /// <param name="startIndex">Позиция относительно начала слова.</param>
        /// <param name="settings">Настройки языка для расстановки переносов в словах.</param>
        /// <param name="pattern">Шаблон.</param>
        /// <param name="levels">Уровни (приоритеты) для расстановки переносов.</param>
        private void SetWordLevels(int wordLength, int startIndex, LanguageSettings settings, Pattern pattern, IDictionary<int, int> levels)
        {
            if (pattern == null || levels == null)
            {
                return;
            }

            foreach (var level in pattern.Levels)
            {
                var key = level.Key + startIndex;

                // settings.MinLeftHyphenLettersCount + 1 - так как в начале слова была добавлена точка.
                // settings.MinLeftHyphenLettersCount + 1 - так как в конце слова была добавлена точка.
                if (key < settings.MinLeftHyphenLettersCount + 1 || wordLength - key < settings.MinLeftHyphenLettersCount + 1)
                {
                    continue;
                }

                if (!levels.ContainsKey(key))
                {
                    levels[key] = level.Value;
                    continue;
                }

                var value = levels[key];
                if (level.Value > value)
                {
                    levels[key] = level.Value;
                }
            }
        }

        #endregion Private Members
    }
}
