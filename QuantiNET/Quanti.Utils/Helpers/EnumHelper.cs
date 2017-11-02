using Quanti.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Quanti.Utils.Helpers
{
    /// <summary>
    /// Represents helper class for an enum type.
    /// </summary>
    public static class EnumHelper<T>
        where T : struct, IComparable, IFormattable, IConvertible
    {
        /// <summary>
        /// Represents localization of enum value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public sealed class TextValue
        {
            /// <summary>
            /// Represents value of enum type.
            /// </summary>
            public T Value { get; internal set; }
            /// <summary>
            /// Localization of value of an enum type.
            /// </summary>
            public string Text { get; internal set; }

            /// <summary>
            /// Creates object representing localization of value of enum type.
            /// It can be created only by methods in <see cref="EnumHelper"/> class.
            /// </summary>
            internal TextValue() { }

            /// <summary>
            /// Creates object representing localization of value of an enum type.
            /// It can be created only by methods in <see cref="EnumHelper"/> class.
            /// </summary>
            /// <param name="value"></param>
            /// <param name="text"></param>
            internal TextValue(T value, string text)
            {
                this.Value = value;
                this.Text = text;
            }
        }

        /// <summary>
        /// Gets localized texts for enum values.
        /// </summary>
        /// <typeparam name="T">Enum type for which values to get localized texts.</typeparam>
        /// <param name="resourcesManager">Resource manager which contains texts which should be used to localize an enum values.</param>
        /// <param name="cultureInfo">Culture info to be used for localization.</param>
        /// <returns></returns>
        public static IEnumerable<TextValue> GetLocalizations(ResourceManager resourcesManager, System.Globalization.CultureInfo cultureInfo)
        {
            ArgumentChecker.Instance
                .ThrowIfNull(resourcesManager, nameof(resourcesManager))
                .ThrowIfNull(cultureInfo, nameof(cultureInfo))
                .ThrowIfNotEnum<T>();

            return GetLocalizations(resourcesManager, defaultValueToIgnore: null, cultureInfo: cultureInfo);
        }

        /// <summary>
        /// Gets localized texts for values of enum type by using specified instance of <see cref="ResourceManager"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resourcesManager"></param>
        /// <param name="defaultValueToIgnore">Value to be filtered from return collection. This way null can be used instead default value.</param>
        /// <param name="cultureInfo">Culture for which to get localization text. If not present then current culture of thread is used.</param>
        /// <returns></returns>
        public static IEnumerable<TextValue> GetLocalizations(ResourceManager resourcesManager, Nullable<T> defaultValueToIgnore = null, System.Globalization.CultureInfo cultureInfo = null)
        {
            ArgumentChecker.Instance
                .ThrowIfNull(resourcesManager, nameof(resourcesManager))
                .ThrowIfNotEnum<T>();

            var type = typeof(T);
            var prefix = type.Name;
            var values = Enum.GetValues(type);

            var texts = new List<TextValue>();
            foreach (T value in values)
            {
                // default value should be filtered out
                if (value.CompareTo(defaultValueToIgnore) == 0)
                    continue;

                var name = Enum.GetName(type, value);
                var resourceName = $"{prefix}_{name}";

#if NETSTANDARD1_6
                string text;
                if (cultureInfo == null)
                {
                    text = resourcesManager.GetString(resourceName);
                }
                else
                {
                    text = resourcesManager.GetString(resourceName, cultureInfo);
                }

                // when resource does not exists for the value return name of the value.
                if (text == null)
                    text = name;
#else
                object resource;
                string text;
                if (cultureInfo == null)
                {
                    resource = resourcesManager.GetObject(resourceName);
                    text = resourcesManager.GetString(resourceName);
                }
                else
                {
                    resource = resourcesManager.GetObject(resourceName, cultureInfo);
                    text = resourcesManager.GetString(resourceName, cultureInfo);
                }

                // when resource does not exists for the value return name of the value.
                if (resource == null)
                    text = name;
#endif

                texts.Add(new TextValue(value, text));
            }

            return texts.OrderBy(t => t.Text);
        }

        /// <summary>
        /// Returns localization texts for an enum by examing each value for <see cref="DisplayAttribute"/>.
        /// If attribute is not available then name of a value is used instead.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TextValue> GetLocalizationsByDisplayAttribute()
        {
            GenericTypeChecker.ThrowIfNotEnum<T>();

            var type = typeof(T);
            var values = Enum.GetValues(type);
            var texts = new List<TextValue>();
            foreach (object objectValue in values)
            {
                var enumValue = (Enum)objectValue;
                var value = (T)objectValue;

                // get name from display attribute if available. Otherwise use name of the value from the type.
                var displayAttribute = enumValue.GetAttribute<DisplayAttribute>();
                var name = (displayAttribute is null) ? Enum.GetName(type, value) : displayAttribute.GetName();

                texts.Add(new TextValue(value, name));
            }

            return texts;
        }
    }
}
