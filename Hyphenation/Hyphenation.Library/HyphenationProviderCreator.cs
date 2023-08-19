namespace Hyphenation.Library
{
    using System;
    using Enums;
    using Interfaces;
    using Strategies;

    // TODO: fix create of instance.
    public class HyphenationProviderCreator : IHyphenationProviderCreator
    {
        #region IHyphenationProviderCreator Members

        public IHyphenationProvider Create(AlgorithmType algorithmType = AlgorithmType.LiangKnuth) =>
            new HyphenationProvider(null)/*CreateProvider(algorithmType)*/;

        #endregion IHyphenationProviderCreator Members

        #region Private Members

        private IHyphenationProvider CreateProvider<TStrategy>() where TStrategy : IHyphenationStrategy, new() =>
            new HyphenationProvider(null/*new TStrategy()*/);

        //private IHyphenationProvider CreateProvider(AlgorithmType algorithmType)
        //{
        //    switch (algorithmType)
        //    {
        //        case AlgorithmType.Christs:
        //            return CreateProvider<ChristsStrategy>();

        //        case AlgorithmType.LiangKnuth:
        //            return CreateProvider<LiangKnuthStrategy>();

        //        default:
        //            throw new ArgumentException(nameof(algorithmType));

        //    }
        //}

        #endregion Private Members
    }
}
