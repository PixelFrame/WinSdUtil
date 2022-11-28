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

    internal static class ArrayExtensions
    {
        public static T[] RangeSubset<T>(this T[] array, int startIndex)
        {
            var length = array.Length - startIndex;
            T[] subset = new T[length];
            Array.Copy(array, startIndex, subset, 0, length);
            return subset;
        }

        public static T[] RangeSubset<T>(this T[] array, int startIndex, int length)
        {
            T[] subset = new T[length];
            Array.Copy(array, startIndex, subset, 0, length);
            return subset;
        }
    }
}