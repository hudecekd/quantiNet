using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Quanti.Utils
{
    public static class GenericTypeChecker
    {
        /// <summary>
        /// Checks that generic type is an enum type.
        /// Since enum types cannot be checked at compile time we need to check at runtime that type we mean to use is actually an enum type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNotEnum<T>()
            where T : IComparable, IFormattable, IConvertible
        {
            if (!typeof(T).GetTypeInfo().IsEnum)
                throw new InvalidOperationException($"Type '{typeof(T).FullName} is not an enum!");
        }
    }
}
