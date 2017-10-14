using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Quanti.WPF.Utils.ValueConverters
{
    [ContentProperty("Converters")]
    public class ComposingConverter : IValueConverter
    {
        private readonly Collection<IValueConverter> _converters = new Collection<IValueConverter>();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Collection<IValueConverter> Converters
        {
            get { return _converters; }
        }

        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            for (int i = 0; i < _converters.Count; i++)
            {
                value = _converters[i].Convert(value, targetType, parameter, culture);
            }

            return value;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            for (int i = _converters.Count - 1; i >= 0; i--)
            {
                value = _converters[i].ConvertBack(value, targetType, parameter, culture);
            }

            return value;
        }
    }
}
