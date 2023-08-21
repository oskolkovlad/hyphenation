namespace Hyphenation.Library.Interfaces.Creators
{
    using Enums;

    public interface IHyphenationProviderCreator
    {
        IHyphenationProvider Create(AlgorithmType algorithmType = AlgorithmType.LiangKnuth);
    }
}
