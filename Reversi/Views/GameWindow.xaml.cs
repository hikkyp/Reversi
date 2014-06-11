using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace Reversi.Views
{
	/// <summary>
	/// GameWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class GameWindow : Window
	{
		public GameWindow ()
		{
			InitializeComponent ();
		}

		private void SlipButton_MouseDown (object sender, MouseButtonEventArgs e)
		{

		}

		private void SlipButton_MouseEnter (object sender, MouseEventArgs e)
		{
			Debug.WriteLine ("ENTER");
		}

		private void SlipButton_MouseLeave (object sender, MouseEventArgs e)
		{
			Debug.WriteLine ("LEAVE");
		}
	}
}
