namespace Hyphenation.Library
{
    using Interfaces;

    internal class HyphenationProvider : IHyphenationProvider
    {
        private readonly IHyphenationStrategy _strategy;

        public HyphenationProvider(IHyphenationStrategy strategy)
        {
            _strategy = strategy;
        }

        #region IHyphenationProvider Members

        public string Insert(string sourceText)
        {
            throw new System.NotImplementedException();
        }

        #endregion IHyphenationProvider Members
    }
}
