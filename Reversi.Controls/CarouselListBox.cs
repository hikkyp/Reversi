using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Reversi.Controls
{
	public class CarouselListBox : ListBox
	{
		#region 非表示メンバ

		static CarouselListBox ()
		{
			DefaultStyleKeyProperty.OverrideMetadata (typeof (CarouselListBox), new FrameworkPropertyMetadata (typeof (CarouselListBox)));
		}

		#endregion

		public static readonly DependencyProperty RotationOrientationProperty = DependencyProperty.Register ("RotationOrientation", typeof (CarouselListBoxRotationOrientation), typeof (CarouselListBox));

		public CarouselListBox ()
		{
		}
		public override void OnApplyTemplate ()
		{
			base.OnApplyTemplate ();
		}
	}
}
