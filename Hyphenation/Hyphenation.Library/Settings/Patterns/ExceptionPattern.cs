namespace Hyphenation.Library.Settings.Patterns
{
    using Extensions;
    using Constants;

    /// <summary>
    /// Класс слова-исключения.
    /// </summary>
    internal class ExceptionPattern
    {
        private static readonly string CurrentHyphenSymbol = Symbols.HyphenSymbol.ToString();

        public ExceptionPattern(string word)
        {
            Word = word;
            InitializeClearWord();
        }

        #region Public Members

        /// <summary>
        /// Слово-исключение с расставленными переносами (дефисами).
        /// </summary>
        public string Word { get; private set; }

        /// <summary>
        /// Слово-исключение, отчищенное от переносов.
        /// </summary>
        public string ClearWord { get; private set; }

        public void ReplaceHyphenSymbol(string hyphenSymbol)
        {
            if (Word != ClearWord && hyphenSymbol != CurrentHyphenSymbol)
            {
                Word = Word.Replace(CurrentHyphenSymbol, hyphenSymbol);
            }
        }

        #endregion Public Members

        #region Private Members

        /// <summary>
        /// Инициализизует слова-исключения, отчищенного от переносов.
        /// </summary>
        private void InitializeClearWord()
        {
            if (!Word.IsNullOrEmpty() && ClearWord == null)
            {
                ClearWord = Word.Replace(CurrentHyphenSymbol, string.Empty);
            }
        }

        #endregion Private Members
    }
}
