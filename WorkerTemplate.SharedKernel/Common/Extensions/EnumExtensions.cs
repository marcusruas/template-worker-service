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
        public static T? GetAttribute<T>(this Enum value) where T : Attribute
        {
            FieldInfo? field = value.GetType().GetField(value.ToString());
            return (T?)field?.GetCustomAttribute(typeof(T), true);
        }

        public static string? StringValueOf(this Enum value)
        {
            FieldInfo? fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[]? attributes = (DescriptionAttribute[]?)fi?.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static IEnumerable<T> ListFrom<T>() where T : Enum
            => Enum.GetValues(typeof(T)).Cast<T>();
    }
}
