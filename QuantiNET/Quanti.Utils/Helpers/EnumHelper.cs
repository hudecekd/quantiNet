using System;
using System.Collections.Generic;
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

        public static IEnumerable<TextValue<T>> GetLocalizations<T>(ResourceManager resourcesManager, System.Globalization.CultureInfo cultureInfo) where T : struct, IConvertible, IComparable
        {
            ArgumentChecker.ThrowIfNull(resourcesManager, nameof(resourcesManager));
            ArgumentChecker.ThrowIfNull(cultureInfo, nameof(cultureInfo));

            return GetLocalizations<T>(resourcesManager, defaultValueToIgnore: null, cultureInfo: cultureInfo);
        }

        /// <summary>
        /// Gets localized texts for values of enum type by using specified instance of <see cref="ResourceManager"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resourcesManager"></param>
        /// <param name="defaultValueToIgnore">Value to be filtered from return collection. This way null can be used instead default value.</param>
        /// <returns></returns>
        public static IEnumerable<TextValue<T>> GetLocalizations<T>(ResourceManager resourcesManager, Nullable<T> defaultValueToIgnore = null, System.Globalization.CultureInfo cultureInfo = null) where T : struct, IConvertible, IComparable
        {
            ArgumentChecker.ThrowIfNull(resourcesManager, nameof(resourcesManager));

            var type = typeof(T);
            if (!type.IsEnum)
                throw new InvalidOperationException($"Type '{type.FullName}' is not enum type!");

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

                texts.Add(new TextValue<T>(value, text));
            }

            return texts.OrderBy(t => t.Text);
        }
    }
}
