namespace Hyphenation.Library.Creators
{
    using System;
    using Enums;
    using Interfaces.Creators;
    using Interfaces.Strategies;
    using Settings;
    using Strategies;

    internal class HyphenationStrategyCreator : IHyphenationStrategyCreator
    {
        private readonly LanguageSettings[] _settings;

        public HyphenationStrategyCreator() =>
            _settings = new LanguageSettings[] { new RussianLanguageSettings(), new EnglishLanguageSettings() };

        #region IHyphenationStrategyCreator Members

        public IHyphenationStrategy Create(AlgorithmType algorithmType) => CreateStrategy(algorithmType);

        #endregion IHyphenationStrategyCreator Members

        #region Private Members

        private IHyphenationStrategy CreateStrategy(AlgorithmType algorithmType)
        {
            switch (algorithmType)
            {
                case AlgorithmType.Christs:
                    return new ChristsStrategy(_settings);

                case AlgorithmType.LiangKnuth:
                    return new LiangKnuthStrategy(_settings);

                default:
                    throw new ArgumentException(nameof(algorithmType));

            }
        }

        #endregion Private Members
    }
}
