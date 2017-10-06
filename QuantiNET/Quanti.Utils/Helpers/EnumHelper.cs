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

        /// <summary>
        /// Gets localized texts for values of enum type by using specified instance of <see cref="ResourceManager"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resourcesManager"></param>
        /// <param name="defaultValueToIgnore">Value to be filtered from return collection. This way null can be used instead default value.</param>
        /// <returns></returns>
        public static IEnumerable<TextValue<T>> GetLocalizations<T>(ResourceManager resourcesManager, Nullable<T> defaultValueToIgnore = null) where T : struct, IConvertible, IComparable
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException($"Type '{typeof(T).FullName}' is not enum type!");

            var prefix = typeof(T).Name;
            var values = Enum.GetValues(typeof(T));

            var texts = new List<TextValue<T>>();
            foreach (T value in values)
            {
                // default value should be filtered out
                if (value.CompareTo(defaultValueToIgnore) == 0)
                    continue;

                var name = Enum.GetName(typeof(T), value);
                var resourceName = $"{prefix}_{name}";
                var text = resourcesManager.GetString(resourceName);

                texts.Add(new TextValue<T>(value, text));
            }

            return texts.OrderBy(t => t.Text);
        }
    }
}
