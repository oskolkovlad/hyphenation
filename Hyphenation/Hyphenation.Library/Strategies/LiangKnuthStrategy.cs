namespace Hyphenation.Library.Strategies
{
    using Hyphenation.Library.Enums;
    using Interfaces;

    internal class LiangKnuthStrategy : BaseStrategy,
        IHyphenationStrategy
    {
        #region IHyphenationStrategy Members

        public AlgorithmType AlgorithmType => throw new System.NotImplementedException();

        public string InsertHyphens(string sourceText)
        {
            throw new System.NotImplementedException();
        }

        #endregion IHyphenationStrategy Members
    }
}
