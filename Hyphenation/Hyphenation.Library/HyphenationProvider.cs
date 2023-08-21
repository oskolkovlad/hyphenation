namespace Hyphenation.Library
{
    using Interfaces;
    using Interfaces.Strategies;

    internal class HyphenationProvider : IHyphenationProvider
    {
        private readonly IHyphenationStrategy _strategy;

        public HyphenationProvider(IHyphenationStrategy strategy)
        {
            _strategy = strategy;
        }

        #region IHyphenationProvider Members

        public string Insert(string sourceText) => _strategy.InsertHyphens(sourceText);

        #endregion IHyphenationProvider Members
    }
}
