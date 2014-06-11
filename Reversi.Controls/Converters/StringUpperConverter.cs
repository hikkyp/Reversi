using System;
using System.Windows.Data;

namespace Reversi.Controls.Converters
{
	public class StringUpperConverter : IValueConverter
	{
		public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return ((string)value).ToUpper ();
		}
		public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException ();
		}
	}
}
