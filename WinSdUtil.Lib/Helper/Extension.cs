namespace WinSdUtil.Lib.Helper
{
    internal static class StringExtensions
    {
        public static IEnumerable<string> SplitInParts(this string s, int partLength)
        {
            if (partLength <= 0)
                throw new ArgumentException("Part length has to be positive.", nameof(partLength));
            for (var i = 0; i < s.Length; i += partLength)
                yield return s.Substring(i, Math.Min(partLength, s.Length - i));
        }

        public static IEnumerable<string> SplitBrackets(this string s, int bracketType)
        {
            char bracketBegin = '(';
            char bracketEnd = ')';
            switch (bracketType)
            {
                case 1: bracketBegin = '['; bracketEnd = ']'; break;
                case 2: bracketBegin = '{'; bracketEnd = '}'; break;
                case 3: bracketBegin = '<'; bracketEnd = '>'; break;
            }
            int breakPos = 0;
            bool endBracket = false;
            for (var i = 0; i < s.Length; i++)
            {
                if (endBracket)
                {
                    endBracket = false;
                    if (s[i] == bracketBegin)
                    {
                        yield return s.Substring(breakPos, i - breakPos);
                        breakPos = i;
                    }
                }
                if (s[i] == bracketEnd) endBracket = true;
                if (i == s.Length - 1)
                    yield return s.Substring(breakPos);
            }
        }
    }
}