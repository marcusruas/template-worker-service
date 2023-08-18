using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace WorkerTemplate.SharedKernel.Common.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Remove accents from a string. returns a new string if the conversion was successfull. If the value is null or whitespace it returns null.
        /// </summary>
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

        /// <summary>
        /// Checks if a string contains accents.
        /// </summary>
        public static bool HasAccents(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            var formattedText = value.Trim().ToUpper();
            var normalizedText = value.RemoveAccents();

            return formattedText != normalizedText;
        }
    }
}
