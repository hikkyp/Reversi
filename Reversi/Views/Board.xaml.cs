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

namespace Reversi.Views
{
	using Core;
	using ViewModels;

	public partial class Board : UserControl
	{
		#region 非表示メンバ

		private GameViewModel _GameViewModel;

		private void _OnLoaded (object sender, RoutedEventArgs e)
		{
			if (_GameViewModel != null) {
				return;
			}
			var width = BoardWidth;
			var height = BoardHeight;
			for (var x = 0; x < width; ++x) {
				BoardGrid.ColumnDefinitions.Add (new ColumnDefinition ());
			}
			for (var y = 0; y < height; ++y) {
				BoardGrid.RowDefinitions.Add (new RowDefinition ());
			}
			for (var y = 0; y < height; ++y) {
				for (var x = 0; x < width; ++x) {
					var boardSpace = new BoardSpace ();
					boardSpace.SetBinding (BoardSpace.SpaceStateProperty, new Binding {
						Path = new PropertyPath (string.Format ("[{0},{1}]", x, y)),
					});
					boardSpace.SetBinding (BoardSpace.CommandProperty, new Binding {
						Path = new PropertyPath ("MoveCommand"),
					});
					boardSpace.CommandParameter = new GameBoardSpace (x, y);
					Grid.SetColumn (boardSpace, x);
					Grid.SetRow (boardSpace, y);
					BoardGrid.Children.Add (boardSpace);
				}
			}
			_GameViewModel = DataContext as GameViewModel;
			if (_GameViewModel == null) {
				return;
			}
		}

		#endregion

		public static readonly DependencyProperty BoardWidthProperty = DependencyProperty.Register ("BoardWidth", typeof (int), typeof (Board), new PropertyMetadata (GameBoardSize.DefaultWidth));
		public static readonly DependencyProperty BoardHeightProperty = DependencyProperty.Register ("BoardHeight", typeof (int), typeof (Board), new PropertyMetadata (GameBoardSize.DefaultHeight));
		public int BoardWidth
		{
			get
			{
				return (int)GetValue (BoardWidthProperty);
			}
			set
			{
				SetValue (BoardWidthProperty, value);
			}
		}
		public int BoardHeight
		{
			get
			{
				return (int)GetValue (BoardHeightProperty);
			}
			set
			{
				SetValue (BoardHeightProperty, value);
			}
		}



		public Board ()
		{
			InitializeComponent ();
			Loaded += _OnLoaded;
		}
	}
}
