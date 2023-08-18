namespace Hyphenation.Library
{
    using System;
    using Enums;
    using Interfaces;
    using Strategies;

    public class HyphenationProviderCreator : IHyphenationProviderCreator
    {
        #region IHyphenationProviderCreator Members

        public IHyphenationProvider Create(AlgorithmType algorithmType = AlgorithmType.LiangKnuth) =>
            CreateProvider(algorithmType);

        #endregion IHyphenationProviderCreator Members

        #region Private Members

        private IHyphenationProvider CreateProvider<TStrategy>() where TStrategy : IHyphenationStrategy, new() =>
            new HyphenationProvider(new TStrategy());

        private IHyphenationProvider CreateProvider(AlgorithmType algorithmType)
        {
            switch (algorithmType)
            {
                case AlgorithmType.Christs:
                    return CreateProvider<ChristsStrategy>();

                case AlgorithmType.LiangKnuth:
                    return CreateProvider<LiangKnuthStrategy>();

                default:
                    throw new ArgumentException(nameof(algorithmType));

            }
        }

        #endregion Private Members
    }
}
