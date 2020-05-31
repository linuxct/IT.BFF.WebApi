using System;
using System.Globalization;
using System.Text;

namespace IT.BFF.Domain.Contracts
{
    public static class StringExtensions
    {
        public static DateTimeOffset? FromTitleWithDate(this string input)
        {
            if (input.Contains('{') && input.Contains('}'))
            {
                var unparsedDate = GetSubstringByString("{", "}", input);
                var dto = DateTimeOffset.ParseExact(unparsedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                return dto;
            }

            return null;
        }
        
        public static string ToTitleWithoutDate(this string input)
        {
            if (input.Contains('{') && input.Contains('}'))
            {
                var unparsedDate = GetSubstringByString("{", "}", input);
                return StringExcept(unparsedDate, input);
            }

            return input;
        }
        
        public static string ToTitleWithDate(this string input, DateTimeOffset dateTime)
        {
            var sb = new StringBuilder();
            var actualTitleContent = input.Contains('{') && input.Contains('}')
                ? input.Substring(input.LastIndexOf('}') + 1)
                : input;

            sb.Append("{");
            sb.Append(dateTime.Date.ToString("yyyy-MM-dd"));
            sb.Append("} ");
            sb.Append(actualTitleContent);
            return sb.ToString();
        }
        
        private static string GetSubstringByString(string firstDelimiter, string secondDelimiter, string input)
        {
            return input.Substring((input.IndexOf(firstDelimiter, StringComparison.Ordinal) + firstDelimiter.Length), 
                (input.IndexOf(secondDelimiter, StringComparison.Ordinal) - input.IndexOf(firstDelimiter, StringComparison.Ordinal) - firstDelimiter.Length));
        }
        
        private static string StringExcept(string unparsedDate, string input)
        {
            var toRemove = "{" + unparsedDate + "}";
            return input.Replace(toRemove, string.Empty).Trim();
        }
    }
}