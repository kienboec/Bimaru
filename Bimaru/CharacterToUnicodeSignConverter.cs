using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Bimaru
{
    public class CharacterToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is char charValue)
            {
                switch (charValue)
                {
                    case 'O':
                        return Brushes.Aqua;
                    case 'X':
                        return Brushes.Brown;
                }

                return Brushes.WhiteSmoke;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
