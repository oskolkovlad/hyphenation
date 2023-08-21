namespace Hyphenation.Library.Settings
{
    using System.Collections.Generic;
    using Enums;
    using Properties;

    internal sealed class RussianLanguageSettings : LanguageSettings
    {
        private const int CurrentMinLettersCount = 4;
        private const int CurrentMinLeftHyphenLettersCount = 2;
        private const int CurrentMinRightHyphenLettersCount = 2;

        #region Symbols and Rules

        private static IReadOnlyList<char> CurrentVowels = new List<char>
        {
            'А',
            'Е',
            'Ё',
            'И',
            'О',
            'У',
            'Ы',
            'Э',
            'Ю',
            'Я'
        };

        private static IReadOnlyList<char> CurrentConsonants = new List<char>
        {
            'Б',
            'В',
            'Г',
            'Д',
            'Ж',
            'З',
            'К',
            'Л',
            'М',
            'Н',
            'П',
            'Р',
            'С',
            'Т',
            'Ф',
            'Х',
            'Ц',
            'Ч',
            'Ш',
            'Щ'
        };

        private static IReadOnlyList<char> CurrentSpecials = new List<char>
        {
            'Й',
            'Ь',
            'Ъ'
        };

        private static IReadOnlyDictionary<string, byte> CurrentRules = new Dictionary<string, byte>
        {
            { "xvv", 1 },
            { "xvc", 1 },
            { "xcv", 1 },
            { "xcc", 1 },

            { "cvcvcx", 2 },

            { "vccccv", 3 },
            { "vcccv", 2 },

            { "cvcv", 2 },
            { "vccv", 2 },
            { "cvvv", 2 },
            { "cvvc", 2 },

            { "vvv", 1 },
            { "vvc", 1 },
            { "vvx", 1 }
        };

        #endregion Symbols and Rules

        #region Public Members

        public override int MinLettersCount => CurrentMinLettersCount;

        public override int MinLeftHyphenLettersCount => CurrentMinLeftHyphenLettersCount;

        public override int MinRightHyphenLettersCount => CurrentMinRightHyphenLettersCount;

        public override string ExceptionsFile => Resources.hyph_ru_hyp;

        public override string PatternsFile => Resources.hyph_ru_pat;

        public override Language Language => Language.Russian;

        public override IReadOnlyCollection<char> Consonants => CurrentConsonants;

        public override IReadOnlyCollection<char> Vowels => CurrentVowels;

        public override IReadOnlyCollection<char> Specials => CurrentSpecials;

        public override IReadOnlyDictionary<string, byte> Rules => CurrentRules;

        #endregion Public Members
    }
}
