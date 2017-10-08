using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Quanti.Utils.Extensions
{
    public static class EnumExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
           where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }

        /// <summary>
        /// Examines value of enum for <see cref="DisplayAttribute"/> and gets localization text from that attribute.
        /// If attribute is not available then null is returned.
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetNameFromDisplayAttribute(this Enum enumValue)
        {
            // get name from display attribute if available. Otherwise use name of the value from the type.
            var type = enumValue.GetType();
            var displayAttribute = enumValue.GetAttribute<DisplayAttribute>();
            if (displayAttribute == null)
                return null;
            return displayAttribute.GetName();
        }
    }
}
