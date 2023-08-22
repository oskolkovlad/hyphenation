namespace Hyphenation.Library.Settings.Patterns
{
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Extensions;

    /// <summary>
    /// Класс шаблонов и слов-исключений языка.
    /// </summary>
    internal class LanguagePatterns
    {
        private readonly LanguageSettings _settings;

        public LanguagePatterns(LanguageSettings settings)
        {
            if (settings == null)
            {
                Exceptions = new List<ExceptionPattern>();
                Patterns = new List<Pattern>();

                return;
            }

            _settings = settings;

            Exceptions = GetExceptionsFromFile().ToList();

            var patterns = GetPatternsFromFile().ToList();
            patterns.Sort();
            Patterns = patterns;
        }

        #region Public Members

        public IList<ExceptionPattern> Exceptions { get; private set; }

        public IList<Pattern> Patterns { get; private set; }

        #endregion Public Members

        #region Private Members

        /// <summary>
        /// Получение слов-исключений из файла исключений TeX.
        /// </summary>
        /// <returns>Слова-исключения.</returns>
        private IEnumerable<ExceptionPattern> GetExceptionsFromFile() =>
            GetLinesFromFile(_settings.ExceptionsFile).Select(word => new ExceptionPattern(word));

        /// <summary>
        /// Получение шаблонов из файла шаблонов TeX.
        /// </summary>
        /// <returns>Шаблоны.</returns>
        private IEnumerable<Pattern> GetPatternsFromFile() =>
            GetLinesFromFile(_settings.PatternsFile).Select(rule => new Pattern(rule, _settings.Language));

        /// <summary>
        /// Получение строк из текствого файла.
        /// </summary>
        /// <param name="stringFile">Текстовый файл.</param>
        /// <returns>Строки.</returns>
        private IEnumerable<string> GetLinesFromFile(string stringFile) =>
            !stringFile.IsNullOrEmpty() ?
                stringFile
                    .Split('\n', '\r', '\t')
                    .Where(pattern => !pattern.StartsWith(Symbols.CommentSymbol)) :
                Enumerable.Empty<string>();

        #endregion Private Members
    }
}
