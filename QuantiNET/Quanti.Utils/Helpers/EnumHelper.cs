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
    public static class EnumHelper
    {
        public class TextValue<T>
        {
            public T Value { get; set; }
            public string Text { get; set; }

            public TextValue() { }

            public TextValue(T value, string text)
            {
                this.Value = value;
                this.Text = text;
            }
        }

        public static IEnumerable<TextValue<T>> GetLocalizations<T>(ResourceManager resourcesManager, System.Globalization.CultureInfo cultureInfo) where T : struct, IComparable, IFormattable, IConvertible
        {
            ArgumentChecker.ThrowIfNull(resourcesManager, nameof(resourcesManager));
            ArgumentChecker.ThrowIfNull(cultureInfo, nameof(cultureInfo));
            GenericTypeChecker.ThrowIfNotEnum<T>();

            return GetLocalizations<T>(resourcesManager, defaultValueToIgnore: null, cultureInfo: cultureInfo);
        }

        /// <summary>
        /// Gets localized texts for values of enum type by using specified instance of <see cref="ResourceManager"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resourcesManager"></param>
        /// <param name="defaultValueToIgnore">Value to be filtered from return collection. This way null can be used instead default value.</param>
        /// <returns></returns>
        public static IEnumerable<TextValue<T>> GetLocalizations<T>(ResourceManager resourcesManager, Nullable<T> defaultValueToIgnore = null, System.Globalization.CultureInfo cultureInfo = null) where T : struct, IComparable, IFormattable, IConvertible
        {
            ArgumentChecker.ThrowIfNull(resourcesManager, nameof(resourcesManager));
            GenericTypeChecker.ThrowIfNotEnum<T>();

            var type = typeof(T);
            var prefix = type.Name;
            var values = Enum.GetValues(type);

            var texts = new List<TextValue<T>>();
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

                texts.Add(new TextValue<T>(value, text));
            }

            return texts.OrderBy(t => t.Text);
        }

        /// <summary>
        /// Returns localization texts for an enum by examing each value for <see cref="DisplayAttribute"/>.
        /// If attribute is not available then name of a value is used instead.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TextValue<T>> GetLocalizationsByDisplayAttribute<T>() where T : struct, IComparable, IFormattable, IConvertible
        {
            GenericTypeChecker.ThrowIfNotEnum<T>();

            var type = typeof(T);
            var values = Enum.GetValues(type);
            var texts = new List<TextValue<T>>();
            foreach (object objectValue in values)
            {
                var enumValue = (Enum)objectValue;
                var value = (T)objectValue;

                // get name from display attribute if available. Otherwise use name of the value from the type.
                var displayAttribute = enumValue.GetAttribute<DisplayAttribute>();
                var name = (displayAttribute is null) ? Enum.GetName(type, value) : displayAttribute.GetName();

                texts.Add(new TextValue<T>(value, name));
            }

            return texts;
        }
    }
}
