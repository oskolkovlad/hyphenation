using System.Collections.Generic;

namespace Hyphenation.Library.Constants
{
    internal static class Symbols
    {
        public const string CommentSymbol = "%";
        public const char HyphenSymbol = '-';

        /// <summary>
        /// Символ мягкого переноса.
        /// </summary>
        public static readonly char SoftHyphenSymbol = (char)173;

        public static readonly char[] TextSeparators =
            { ',', '.', ':', ';', '"', '/', '[', ']', '(', ')', '<', '>', '|', '?', '!', '@', '#', '&', '%', '+', '^', '$', '*' };
        public static readonly char[] WordSymbols = { '\'', '`', HyphenSymbol };

        public static readonly IReadOnlyDictionary<string, string> EscapeSymbols = new Dictionary<string, string>
        {
            { @"\", @"\\" },
            { "^", @"\^" },
            { "$", @"\$" },
            { ".", @"\." },
            { "+", @"\+" },
            { "*", @"\*" },
            { "?", @"\?" },
            { "[", @"\[" },
            { "]", @"\]" },
            { "(", @"\(" },
            { ")", @"\)" },
            { "|", @"\|" },
        };
    }
}
