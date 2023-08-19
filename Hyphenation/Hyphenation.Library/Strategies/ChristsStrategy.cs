namespace Hyphenation.Library.Strategies
{
    using Hyphenation.Library.Enums;
    using Hyphenation.Library.Settings;
    using Interfaces;

    /// <summary>
    /// Класс, реализующий алгоритм расстановки переносов в словах (алгоритм Христова в модификации Дымченко и Варсанофьева).
    /// </summary>
    internal sealed class ChristsStrategy : BaseStrategy,
        IHyphenationStrategy
    {
        public ChristsStrategy(LanguageSettings[] settingsArray) : base(settingsArray) { }

        #region IHyphenationStrategy Members

        public override AlgorithmType Type => AlgorithmType.Christs;

        #endregion IHyphenationStrategy Members

        #region Protected Members

        protected override string PerformInsertHyphensInWord(string sourceWord, string hyphenSymbol, LanguageSettings settings)
        {
            throw new System.NotImplementedException();
        }

        #endregion Protected Members
    }
}
