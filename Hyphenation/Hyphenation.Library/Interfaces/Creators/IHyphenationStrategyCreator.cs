namespace Hyphenation.Library.Interfaces.Creators
{
    using Enums;
    using Interfaces.Strategies;

    internal interface IHyphenationStrategyCreator
    {
        IHyphenationStrategy Create(AlgorithmType algorithmType);
    }
}
