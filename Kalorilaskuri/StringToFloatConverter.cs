using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Kalorilaskuri
{
    public class StringToFloatConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType,
            object parameter,
    System.Globalization.CultureInfo culture)
        {
            // value is the data from the source object.
            
            string input = value.ToString();
            float f;
            input.Replace(",", ".");
            float.TryParse(input, out f);
            return f.ToString("n2");
            
            //return value.ToString();
            
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            string input = value.ToString();
            MessageBox.Show("convertback " + input);
            float f;
            input.Replace(",", ".");
            float.TryParse(input, out f);
            return f;
        }

        #endregion
    }
}
