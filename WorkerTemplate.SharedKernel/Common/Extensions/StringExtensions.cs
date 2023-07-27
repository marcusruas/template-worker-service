using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace WorkerTemplate.SharedKernel.Common.Extensions
{
    public static class StringExtensions
    {
        public static string? RemoveAccents(this string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            var stringBuilder = new StringBuilder();

            var stringNormalizada = value.Normalize(NormalizationForm.FormD);
            foreach (var character in stringNormalizada)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(character);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(character);
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC).Trim();
        }

        public static bool HasWhiteSpaces(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            foreach (var character in text)
            {
                if (char.IsWhiteSpace(character))
                    return true;
            }

            return false;
        }

        public static bool HasAccents(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            var formattedText = text.Trim().ToUpper();
            var normalizedText = text.RemoveAccents();

            return formattedText != normalizedText;
        }
    }
}
