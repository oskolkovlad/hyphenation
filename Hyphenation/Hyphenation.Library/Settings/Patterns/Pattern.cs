namespace Hyphenation.Library.Settings.Patterns
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Enums;
    using Extensions;

    /// <summary>
    /// Класс шаблона.
    /// </summary>
    internal class Pattern : IComparable
    {
        private const char RussianELetter = 'е';
        private const char RussianZhLetter = 'ж';
        private const char RussianYoLetter = 'ё';

        private readonly Language _language;

        public Pattern(string rule, Language language)
        {
            Rule = rule;
            _language = language;

            InitializeStringRule();
            InitializeLevels();
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            var pattern = obj as Pattern;
            if (pattern == null)
            {
                throw new ArgumentException("Некорректное значение параметра obj.");
            }

            if (StringRule == pattern.StringRule)
            {
                return 0;
            }

            var isBefore = StringRule.Length < pattern.StringRule.Length;
            var minLength = isBefore ? StringRule.Length : pattern.StringRule.Length;

            for (var i = 0; i < minLength; i++)
            {
                var currentRuleCharacter = StringRule[i];
                var externRuleCharacter = pattern.StringRule[i];

                if (_language == Language.Russian && CheckRussianCustomCondition(currentRuleCharacter, externRuleCharacter, out var result))
                {
                    return result;
                }

                if (currentRuleCharacter < externRuleCharacter)
                {
                    return -1;
                }

                if (currentRuleCharacter > externRuleCharacter)
                {
                    return 1;
                }
            }

            return isBefore ? -1 : 1;
        }

        #endregion IComparable Members

        #region Public Members

        /// <summary>
        /// Исходное правило.
        /// </summary>
        public string Rule { get; private set; }

        /// <summary>
        /// Строковое правило, отчищенное от уровней.
        /// </summary>
        public string StringRule { get; private set; }

        /// <summary>
        /// Уровни (приоритет) для расстановки переносов.
        /// </summary>
        /// /// <summary>
        /// Словарь уровней (приоритет правил), где
        /// key - позиция для вставки символа переноса, относительно строкового правила, отчищенного от уровней;
        /// value - значение уровня (приоритет).
        /// </summary>
        public IReadOnlyDictionary<int, int> Levels { get; private set; }

        #endregion Public Members

        #region Private Members

        /// <summary>
        /// Проверяет специальные условия для русского языка (метод CompareTo):
        /// - корректная сортировка по алфавиту - неправильная сортировка при наличии буквы 'ё' (необязательное условие).
        /// </summary>
        /// <param name="currentRuleCharacter">Буква текущего правила.</param>
        /// <param name="externRuleCharacter">Буква внешнего правила.</param>
        /// <param name="result">Результат.</param>
        /// <returns>Результат проверки: если результат (result) был получен, то "true".</returns>
        private bool CheckRussianCustomCondition(char currentRuleCharacter, char externRuleCharacter, out int result)
        {
            result = 666; // Значением по умолчанию может служить любое число, отличное от -1, 0 и 1.

            if (currentRuleCharacter == RussianYoLetter && externRuleCharacter != RussianYoLetter)
            {
                if (externRuleCharacter >= RussianZhLetter)
                {
                    result = -1;
                    return true;
                }

                if (externRuleCharacter <= RussianELetter)
                {
                    result = 1;
                    return true;
                }
            }

            if (currentRuleCharacter != RussianYoLetter && externRuleCharacter == RussianYoLetter)
            {
                if (currentRuleCharacter <= RussianELetter)
                {
                    result = -1;
                    return true;
                }

                if (currentRuleCharacter >= RussianZhLetter)
                {
                    result = 1;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Инициализизует уровни (приоритеты) для расстановки переносов. 
        /// </summary>
        private void InitializeLevels()
        {
            if (Rule.IsNullOrEmpty() || StringRule.IsNullOrEmpty() || Levels != null)
            {
                return;
            }

            var levels = new Dictionary<int, int>();

            for (var position = 0; position < Rule.Length; position++)
            {
                var character = Rule[position];
                if (char.IsNumber(character))
                {
                    levels.Add(position - levels.Count, (int)char.GetNumericValue(character));
                }
            }

            Levels = levels;
        }

        /// <summary>
        /// Инициализизует строковое правило, отчищенное от уровней.
        /// </summary>
        private void InitializeStringRule()
        {
            if (!Rule.IsNullOrEmpty() && StringRule == null)
            {
                StringRule = Regex.Replace(Rule, @"[\d]", string.Empty);
            }
        }

        #endregion Private Members
    }
}
