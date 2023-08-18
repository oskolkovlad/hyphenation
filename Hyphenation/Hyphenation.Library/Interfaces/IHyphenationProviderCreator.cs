using Hyphenation.Library.Enums;

namespace Hyphenation.Library.Interfaces
{
    public interface IHyphenationProviderCreator
    {
        IHyphenationProvider Create(AlgorithmType algorithmType = AlgorithmType.LiangKnuth);
    }
}
