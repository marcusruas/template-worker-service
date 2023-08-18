using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkerTemplate.SharedKernel.Common.Extensions
{
    public static class Helpers
    {
        /// <summary>
        /// Checks if the IEnumerable<typeparamref name="T"/>> is null or its length equals 0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> value)
            => value is null || value.Count() == 0;
    }
}