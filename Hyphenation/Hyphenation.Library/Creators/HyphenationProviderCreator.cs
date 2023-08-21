namespace Hyphenation.Library.Creators
{
    using Enums;
    using Interfaces;
    using Interfaces.Creators;

    public class HyphenationProviderCreator : IHyphenationProviderCreator
    {
        private readonly IHyphenationStrategyCreator _hyphenationStrategyCreator;

        public HyphenationProviderCreator() => _hyphenationStrategyCreator = new HyphenationStrategyCreator();

        #region IHyphenationProviderCreator Members

        public IHyphenationProvider Create(AlgorithmType algorithmType = AlgorithmType.LiangKnuth)
        {
            var strategy = _hyphenationStrategyCreator.Create(algorithmType);
            return new HyphenationProvider(strategy);
        }

        #endregion IHyphenationProviderCreator Members
    }
}
