using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanti.Utils.Helpers
{ 
    public static class MathHelper
    {
        /// <summary>
        /// Converts value to integer if possible. If rounding should be done then exception is thrown.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ConverToInteger(decimal value)
        {
            if ((int)value != value) throw new ArgumentException(string.Format("Value '{0}' cannot be converted to integer without rounding!'{0}'"));
            return (int)value;
        }
    }
}
