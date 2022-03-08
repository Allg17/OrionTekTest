using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace OrionTekTestALG.ViewModels.utils
{
    public class SexoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                int.TryParse(value.ToString(), out int res);
                if (res == 0)
                {
                    return "Masculino";
                }
                else
                    return "Femenino";
            }
            else
                return "Error!";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
