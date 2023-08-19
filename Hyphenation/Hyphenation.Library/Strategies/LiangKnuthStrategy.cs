namespace Hyphenation.Library.Strategies
{
    using Hyphenation.Library.Enums;
    using Hyphenation.Library.Settings;
    using Interfaces;

    /// <summary>
    /// Класс, реализующий алгоритм расстановки переносов в словах (алгоритм Ляна-Кнута).
    /// 
    /// В шаблоне расстановки переносов в TeX есть три вида символов.
    /// Точка - является привязкой к границе слова.
    /// Буква - обозначает саму себя, то есть букву в слове, которое переносится через дефис.
    /// Число - обозначает потенциальную точку переноса, а число обозначает уровень переноса.
    /// 
    /// Основная идея алгоритма заключается в том, что слово сопоставляется с шаблонами и на их основе заполняется уровнями.
    /// Если два уровня из двух разных шаблонов совпадают в одной и той же точке, выбирается более высокий.
    /// Из конечных значений только нечетные уровни указывают на допустимые точки переноса. Идея состоит в том, чтобы иметь возможность указывать
    /// как возможные точки переноса, так и места, где дефис не следует вставлять. Так, например, если определенное место в слове соответствует
    /// двум шаблонам, в которых есть 1 и 2 в этом месте, перенос в этой точке запрещен, потому что 2 переопределяет 1, и только нечетное значение
    /// указывает на разрешенную точку расстановки переносов.
    /// </summary>
    internal sealed class LiangKnuthStrategy : BaseStrategy,
        IHyphenationStrategy
    {
        public LiangKnuthStrategy(LanguageSettings[] settingsArray) : base(settingsArray) { }

        #region IHyphenationStrategy Members

        public override AlgorithmType Type => AlgorithmType.LiangKnuth;

        #endregion IHyphenationStrategy Members

        #region Protected Members

        protected override string PerformInsertHyphensInWord(string sourceWord, string hyphenSymbol, LanguageSettings settings)
        {
            throw new System.NotImplementedException();
        }

        #endregion Protected Members
    }
}
