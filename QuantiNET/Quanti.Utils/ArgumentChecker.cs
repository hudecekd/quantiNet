using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Quanti.Utils
{
    public class ArgumentChecker
    {
        private static Lazy<ArgumentChecker> lazyInstance = new Lazy<ArgumentChecker>();
        public static ArgumentChecker Instance { get => lazyInstance.Value; }

        protected ArgumentChecker()
        {

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArgumentChecker ThrowIfNull<T>(T argument, string argumentName)
            where T : class
        {
            if (argument == null)
                throw new ArgumentNullException(argumentName);

            return Instance;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArgumentChecker ThrowIfNotEnum<T>()
            where T : struct, IComparable, IFormattable, IConvertible
        {
            GenericTypeChecker.ThrowIfNotEnum<T>();
            return Instance;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfOutOfRange(double argument, string argumentName, double? minimum = null, double? maximum = null)
        {
            if ((minimum != null) && (maximum != null))
            {
                if ((argument < minimum) || (argument > maximum))
                    throw new ArgumentOutOfRangeException(argumentName);
            }

            if (minimum != null)
            {
                if (argument < minimum)
                    throw new ArgumentOutOfRangeException(argumentName);
            }

            if (maximum != null)
            {
                if (argument > maximum)
                    throw new ArgumentOutOfRangeException(argumentName);
            }

            // no range specified => range is good
        }
    }
}
