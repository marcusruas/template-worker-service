using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WorkerTemplate.SharedKernel.Common.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets an attribute implemented by the Enum class
        /// </summary>
        public static T? GetAttribute<T>(this Enum value) where T : Attribute
        {
            FieldInfo? field = value.GetType().GetField(value.ToString());
            return (T?)field?.GetCustomAttribute(typeof(T), true);
        }

        /// <summary>
        /// Returns the DescriptionAttribute value in the Enum class. returns the ToString of the Enum if it doesn't implement the DescriptionAttribute
        /// </summary>
        public static string? StringValueOf(this Enum value)
        {
            FieldInfo? fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[]? attributes = (DescriptionAttribute[]?)fi?.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        /// <summary>
        /// Generates a list with all the values of the Enum
        /// </summary>
        public static IEnumerable<T> ListFrom<T>() where T : Enum
            => Enum.GetValues(typeof(T)).Cast<T>();
    }
}
