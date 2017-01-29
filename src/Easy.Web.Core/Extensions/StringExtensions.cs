namespace Easy.Web.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Easy.Web.Core.Helpers;

    /// <summary>
    /// Provides a set of helper methods for working with <see cref="string"/>.
    /// </summary>
    internal static class StringExtensions
    {
        private const StringComparison FastCmpPlcy = StringComparison.OrdinalIgnoreCase;
        private static readonly char[] RegexCharacters = { 'G', 'Z', 'A', 'n', 'W', 'w', 'v', 't', 's', 'S', 'r', 'k', 'f', 'D', 'd', 'B', 'b' };
        private static readonly char[] CommaChar = { ',' };
        private static readonly char[] SpaceChar = { ' ' };
        private const string SpaceString = " ";
        private const string CommaString = ",";
        private const string SemiColonString = ";";

        private static readonly Regex FormUrlEncodedMediaTypeRegex = 
            new Regex($"{MediaTypes.FormUrlEncoded.ToCaseIncensitiveRegexArgument()}", RegexOptions.Compiled);

        private static readonly Regex BinaryMediaTypeRegex = 
            new Regex($"{MediaTypes.Binary.ToCaseIncensitiveRegexArgument()}", RegexOptions.Compiled);

        private static readonly Regex JSONMediaTypeRegex =
            new Regex($"({MediaTypes.JSON.ToCaseIncensitiveRegexArgument()}|{"text/json".ToCaseIncensitiveRegexArgument()}|{"application/vnd\\.\\S*\\+json".ToCaseIncensitiveRegexArgument()})", RegexOptions.Compiled);

        private static readonly Regex XMLMediaTypeRegex =
            new Regex($"({MediaTypes.XML.ToCaseIncensitiveRegexArgument()}|{"text/xml".ToCaseIncensitiveRegexArgument()}|{"application/vnd\\.\\S*\\+xml".ToCaseIncensitiveRegexArgument()})", RegexOptions.Compiled);

        private static readonly Regex CharacterSetRegex =
            new Regex($"{"charset".ToCaseIncensitiveRegexArgument()}\\s?=\\s?(\\S+[^;]);?$", RegexOptions.Compiled);

        /// <summary>
        /// Attempts to extract the <c>charset</c> from the given <paramref name="input"/> 
        /// e.g. <c>UTF-8</c> in <c>application/json; charset=UTF-8</c>.
        /// </summary>
        [DebuggerStepThrough]
        internal static bool TryExtractCharacterSet(this string input, out string characterSet)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                characterSet = null;
                return false;
            }

            var match = CharacterSetRegex.Match(input);

            if (!match.Success)
            {
                characterSet = null;
                return false;
            }

            characterSet = match.Groups[1].Value;
            return true;

        }

        /// <summary>
        /// Returns a flag indicating whether the given <paramref name="mediaType"/> is a <c>application/x-www-form-urlencoded</c> media type.
        /// </summary>
        [DebuggerStepThrough]
        internal static bool IsFormUrlEncodedMediaType(this string mediaType)
        {
            return !string.IsNullOrEmpty(mediaType) && FormUrlEncodedMediaTypeRegex.IsMatch(mediaType);
        }

        /// <summary>
        /// Returns a flag indicating whether the given <paramref name="mediaType"/> is a <c>JSON</c> media type.
        /// </summary>
        [DebuggerStepThrough]
        internal static bool IsJSONMediaType(this string mediaType)
        {
            return !string.IsNullOrEmpty(mediaType) && JSONMediaTypeRegex.IsMatch(mediaType);
        }

        /// <summary>
        /// Returns a flag indicating whether the given <paramref name="mediaType"/> is a <c>XML</c> media type.
        /// </summary>
        [DebuggerStepThrough]
        internal static bool IsXMLMediaType(this string mediaType)
        {
            return !string.IsNullOrEmpty(mediaType) && XMLMediaTypeRegex.IsMatch(mediaType);
        }

        /// <summary>
        /// Returns a flag indicating whether the given <paramref name="mediaType"/> is a <c>XML</c> media type.
        /// </summary>
        [DebuggerStepThrough]
        internal static bool IsBinaryMediaType(this string mediaType)
        {
            return !string.IsNullOrEmpty(mediaType) && BinaryMediaTypeRegex.IsMatch(mediaType);
        }

        /// <summary>
        /// Extracts the media types from the given <paramref name="header"/>.
        /// <example>
        /// Retrieves <c>application/json</c> and <c>text/xml</c> from <c>application/json, text/html; charset=utf-8</c>.
        /// </example>
        /// </summary>
        [DebuggerStepThrough]
        internal static IEnumerable<string> ExtractMediaTypes(this string header)
        {
            if (header.Contains(CommaString))
            {
                // more than one
                foreach (var mediaType in header.Split(CommaChar, StringSplitOptions.RemoveEmptyEntries))
                {
                    yield return mediaType.CleanMediaType();
                }
            } else
            {
                yield return header.CleanMediaType();
            }
        }

        [DebuggerStepThrough]
        internal static string CleanMediaType(this string mediaType)
        {
            var cleaned = mediaType;
            if (cleaned.StartsWith(SpaceString, FastCmpPlcy))
            {
                cleaned = cleaned.TrimStart(SpaceChar);
            }

            if (cleaned.EndsWith(SpaceString, FastCmpPlcy))
            {
                cleaned = cleaned.TrimEnd(SpaceChar);
            }

            if (cleaned.Contains(SemiColonString))
            {
                var endIdx = cleaned.IndexOf(SemiColonString, FastCmpPlcy);
                cleaned = cleaned.Substring(0, endIdx);
            }

            return cleaned;
        }

        /// <summary>
        /// Converts <c>API</c> to <c>[aA][pP][iI]</c>.
        /// <remarks>
        /// This should be used as much faster alternative to adding <see cref="RegexOptions.IgnoreCase"/> 
        /// or using the <c>(?i)</c> for example <c>(?i)API(?-i)</c>
        /// </remarks>
        /// </summary>
        [DebuggerStepThrough]
        internal static string ToCaseIncensitiveRegexArgument(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) { return input; }

            var patternIndexes = input.GetStartAndEndIndexes("(?<", ">").ToArray();
            var hasPattern = patternIndexes.Length > 0;
            var isInPattern = false;

            var builder = StringBuilderCache.Acquire();
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < input.Length; i++)
            {
                var prev = i == 0 ? new char() : input[i - 1];
                var currChar = input[i];

                if (hasPattern)
                {
                    foreach (var pair in patternIndexes)
                    {
                        if (i >= pair.Key && i <= pair.Value)
                        {
                            isInPattern = true;
                            break;
                        }

                        isInPattern = false;
                    }
                }

                if (!char.IsLetter(currChar)
                    || (prev == '\\' && RegexCharacters.Contains(currChar))
                    || isInPattern)
                {
                    builder.Append(currChar);
                    continue;
                }

                builder.Append('[');

                if (char.IsUpper(currChar))
                {
                    builder.Append(char.ToLower(currChar)).Append(currChar);
                }
                else
                {
                    builder.Append(currChar).Append(char.ToUpper(currChar));
                }
                builder.Append(']');
            }
            return StringBuilderCache.GetStringAndRelease(builder);
        }

        /// <summary>
        /// Returns the all the start and end indexes of the occurrences of the 
        /// given <paramref name="startTag"/> and <paramref name="endTag"/> 
        /// in the given <paramref name="input"/>.
        /// </summary>
        /// <param name="input">The input to search.</param>
        /// <param name="startTag">The starting tag e.g. <c>&lt;div></c>.</param>
        /// <param name="endTag">The ending tag e.g. <c>&lt;/div></c>.</param>
        /// <returns>
        /// A sequence <see cref="KeyValuePair{TKey,TValue}"/> where the key is 
        /// the starting position and value is the end position.
        /// </returns>
        [DebuggerStepThrough]
        internal static IEnumerable<KeyValuePair<int, int>> GetStartAndEndIndexes(this string input, string startTag, string endTag)
        {
            var startIdx = 0;
            int endIdx;

            while ((startIdx = input.IndexOf(startTag, startIdx, StringComparison.Ordinal)) != -1
                && (endIdx = input.IndexOf(endTag, startIdx, StringComparison.Ordinal)) != -1)
            {
                var result = new KeyValuePair<int, int>(startIdx, endIdx);
                startIdx = endIdx;
                yield return result;
            }
        }

        /// <summary>
        /// Attempts to convert the given <paramref name="value"/> to the given <paramref name="type"/>.
        /// </summary>
        [DebuggerStepThrough]
        internal static bool TryConvert(this string value, Type type, out object result)
        {
            bool converted;
            result = null;

            var propertyType = Nullable.GetUnderlyingType(type) ?? type;

            if (propertyType == ConversionTypes[0])
            {
                result = value;
                return true;
            }

            if (propertyType == ConversionTypes[1])
            {
                short tmpResult;
                converted = short.TryParse(value, out tmpResult);
                if (converted) { result = tmpResult; }
                return converted;
            }

            if (propertyType == ConversionTypes[2])
            {
                ushort tmpResult;
                converted = ushort.TryParse(value, out tmpResult);
                if (converted) { result = tmpResult; }
                return converted;
            }

            if (propertyType == ConversionTypes[3])
            {
                int tmpResult;
                converted = int.TryParse(value, out tmpResult);
                if (converted) { result = tmpResult; }
                return converted;
            }

            if (propertyType == ConversionTypes[4])
            {
                uint tmpResult;
                converted = uint.TryParse(value, out tmpResult);
                if (converted) { result = tmpResult; }
                return converted;
            }

            if (propertyType == ConversionTypes[5])
            {
                long tmpResult;
                converted = long.TryParse(value, out tmpResult);
                if (converted) { result = tmpResult; }
                return converted;
            }

            if (propertyType == ConversionTypes[6])
            {
                ulong tmpResult;
                converted = ulong.TryParse(value, out tmpResult);
                if (converted) { result = tmpResult; }
                return converted;
            }

            if (propertyType == ConversionTypes[7])
            {
                bool tmpResult;
                converted = bool.TryParse(value, out tmpResult);
                if (converted) { result = tmpResult; }
                return converted;
            }

            if (propertyType == ConversionTypes[8])
            {
                float tmpResult;
                converted = float.TryParse(value, out tmpResult);
                if (converted) { result = tmpResult; }
                return converted;
            }

            if (propertyType == ConversionTypes[9])
            {
                double tmpResult;
                converted = double.TryParse(value, out tmpResult);
                if (converted) { result = tmpResult; }
                return converted;
            }

            if (propertyType == ConversionTypes[10])
            {
                decimal tmpResult;
                converted = decimal.TryParse(value, out tmpResult);
                if (converted) { result = tmpResult; }
                return converted;
            }

            if (propertyType == ConversionTypes[11])
            {
                DateTime tmpResult;
                converted = DateTime.TryParse(value, out tmpResult);
                if (converted) { result = tmpResult; }
                return converted;
            }

            if (propertyType == ConversionTypes[12])
            {
                DateTimeOffset tmpResult;
                converted = DateTimeOffset.TryParse(value, out tmpResult);
                if (converted) { result = tmpResult; }
                return converted;
            }

            if (propertyType == ConversionTypes[13])
            {
                TimeSpan tmpResult;
                converted = TimeSpan.TryParse(value, out tmpResult);
                if (converted) { result = tmpResult; }
                return converted;
            }

            return false;
        }

        private static readonly Type[] ConversionTypes = {
            typeof(string),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(bool),
            typeof(float),
            typeof(double),
            typeof(decimal),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan)
        };
    }
}