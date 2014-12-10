using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Crane.Core.Utility
{
    public static class StringExtensions
    {
        private static readonly ThreadLocal<Random> Random =
            new ThreadLocal<Random>(() => new Random((int)DateTime.Now.Ticks));

        public static bool StartsWithNumber(this string source)
        {
            return Char.IsDigit(source.ToCharArray()[0]);
        }

        public static int ToInt(this string source)
        {
            string a = source;
            string b = string.Empty;

            for (int i = 0; i < a.Length; i++)
            {
                if (Char.IsDigit(a[i]))
                    b += a[i];
            }

            if (b.Length > 0)
                return int.Parse(b);

            return 0;
        }

        public static float ToFloat(this string source)
        {
            string raw = ExtractNumber(source);

            if (raw.Length > 0)
                return float.Parse(raw);

            return 0;
        }

        public static string ToPascalCase(this string source)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(source.ToLower());
        }

        public static string PascalCaseToWords(this string source)
        {
            var sb = new StringBuilder();
            bool firstWord = true;

            foreach (object match in Regex.Matches(source, "([A-Z][a-z]+)|[0-9]+"))
            {
                if (firstWord)
                {
                    sb.Append(match);
                    firstWord = false;
                }
                else
                {
                    sb.Append(" ");
                    sb.Append(match.ToString().ToLower());
                }
            }

            return sb.ToString();
        }

        public static string ExtractVersion(this string source)
        {
            return ExtractNumber(source, allowMultipleDecimalPoints: true);
        }

        public static bool Contains(this string source, string value, StringComparison comparison)
        {
            switch (comparison)
            {
            case StringComparison.CurrentCultureIgnoreCase:
            case StringComparison.InvariantCultureIgnoreCase:
            case StringComparison.OrdinalIgnoreCase:
                if (source == null)
                {
                    return false;
                }

                if (value == null)
                {
                    return false;
                }

                return source.ToLower().Contains(value.ToLower());
            default:
                return source.Contains(value);
            }
        }

        private static string ExtractNumber(string source, bool allowMultipleDecimalPoints = false)
        {
            var result = new StringBuilder();
            bool decimalAdded = false;
            for (int i = 0; i < source.Length; i++)
            {
                char value = source[i];

                bool isDecimalPoint = value == '.';
                if ((!Char.IsDigit(value) && !isDecimalPoint) && result.Length > 0)
                    break;

                if (Char.IsDigit(value) || (isDecimalPoint && result.Length > 0 && !decimalAdded))
                {
                    if (isDecimalPoint && !allowMultipleDecimalPoints)
                    {
                        decimalAdded = true;
                    }

                    result.Append(value);
                }
            }

            return result.ToString().TrimEnd('.');
        }

        public static string RandomString(int size)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                char ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * Random.Value.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public static string Replace(this string value, string oldValue, string newValue, StringComparison comparison)
        {
            var sb = new StringBuilder();

            int previousIndex = 0;
            int index = value.IndexOf(oldValue, comparison);
            while (index != -1)
            {
                sb.Append(value.Substring(previousIndex, index - previousIndex));
                sb.Append(newValue);
                index += oldValue.Length;

                previousIndex = index;
                index = value.IndexOf(oldValue, index, comparison);
            }
            sb.Append(value.Substring(previousIndex));

            return sb.ToString();
        }

        public static string FileNameExtension(this string value)
        {
            var info = new FileInfo(value);
            return info.Extension;
        }

        public static string NameWithoutExtension(this string value)
        {
            var info = new FileInfo(value);
            return info.Name.Replace(info.Extension, string.Empty);
        }
    }

}