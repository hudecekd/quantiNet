using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Quanti.Utils.Extensions
{
    /// <summary>
    /// Represents class which contains extension methods for values of enum type.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets an attribute for a value of an enum type. If attributes is not present then exceptions is thrown.
        /// </summary>
        /// <typeparam name="TAttribute">Attribute to be retrieved.</typeparam>
        /// <param name="enumValue">Enum value for which attribute should be retrieved.</param>
        /// <returns></returns>
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetTypeInfo()
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
