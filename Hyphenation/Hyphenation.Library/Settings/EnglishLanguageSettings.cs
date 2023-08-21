namespace Hyphenation.Library.Settings
{
    using System.Collections.Generic;
    using Enums;
    using Properties;

    internal sealed class EnglishLanguageSettings : LanguageSettings
    {
        private const int CurrentMinLettersCount = 4;
        private const int CurrentMinLeftHyphenLettersCount = 2;
        private const int CurrentMinRightHyphenLettersCount = 3;

        #region Symbols and Rules

        private static IReadOnlyList<char> CurrentVowels = new List<char>
        {
            'A',
            'E',
            'I',
            'U',
            'O',
            'Y'
        };

        private static IReadOnlyList<char> CurrentConsonants = new List<char>
        {
            'B',
            'C',
            'D',
            'F',
            'G',
            'H',
            'J',
            'K',
            'L',
            'M',
            'N',
            'P',
            'Q',
            'R',
            'S',
            'T',
            'V',
            'W',
            'X',
            'Z'
        };

        private static IReadOnlyList<char> CurrentSpecials = new List<char>();

        private static IReadOnlyDictionary<string, byte> CurrentRules = new Dictionary<string, byte>
        {
            { "vccccv", 3 },
            { "vcccv", 2 },

            { "cvcv", 2 },
            { "vccv", 2 },
            { "cvvv", 2 },
            { "cvvc", 2 }
        };

        #endregion Symbols and Rules

        #region Public Members

        public override int MinLettersCount => CurrentMinLettersCount;

        public override int MinLeftHyphenLettersCount => CurrentMinLeftHyphenLettersCount;

        public override int MinRightHyphenLettersCount => CurrentMinRightHyphenLettersCount;

        public override string ExceptionsFile => Resources.hyph_en_us_hyp;

        public override string PatternsFile => Resources.hyph_en_us_pat;

        public override Language Language => Language.English;

        public override IReadOnlyCollection<char> Consonants => CurrentConsonants;

        public override IReadOnlyCollection<char> Vowels => CurrentVowels;

        public override IReadOnlyCollection<char> Specials => CurrentSpecials;

        public override IReadOnlyDictionary<string, byte> Rules => CurrentRules;

        #endregion Public Members
    }
}
