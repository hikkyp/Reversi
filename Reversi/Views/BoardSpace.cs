using System.Windows;
using System.Windows.Controls;

namespace Reversi.Views
{
	using ViewModels;

	public class BoardSpace : Button
	{
		#region 非表示メンバ

		static BoardSpace ()
		{
			DefaultStyleKeyProperty.OverrideMetadata (typeof (BoardSpace), new FrameworkPropertyMetadata (typeof (BoardSpace)));
		}
		private void _UpdateLayout (bool useTransitions)
		{
			switch (SpaceState) {
			case BoardSpaceState.Empty: {
					VisualStateManager.GoToState (this, "Empty", useTransitions);
					VisualStateManager.GoToState (this, "NoIndicator", useTransitions);
					break;
				}
			case BoardSpaceState.Black: {
					VisualStateManager.GoToState (this, "Black", useTransitions);
					VisualStateManager.GoToState (this, "NoIndicator", useTransitions);
					break;
				}
			case BoardSpaceState.White: {
					VisualStateManager.GoToState (this, "White", useTransitions);
					VisualStateManager.GoToState (this, "NoIndicator", useTransitions);
					break;
				}
			case BoardSpaceState.BlackHint: {
					VisualStateManager.GoToState (this, "Black", useTransitions);
					VisualStateManager.GoToState (this, "NoIndicator", useTransitions);
					break;
				}
			case BoardSpaceState.WhiteHint: {
					VisualStateManager.GoToState (this, "White", useTransitions);
					VisualStateManager.GoToState (this, "NoIndicator", useTransitions);
					break;
				}
			case BoardSpaceState.BlackNewPiece: {
					VisualStateManager.GoToState (this, "Black", useTransitions);
					VisualStateManager.GoToState (this, "NewPiece", useTransitions);
					break;
				}
			case BoardSpaceState.WhiteNewPiece: {
					VisualStateManager.GoToState (this, "White", useTransitions);
					VisualStateManager.GoToState (this, "NewPiece", useTransitions);
					break;
				}
			case BoardSpaceState.BlackNewCapture: {
					VisualStateManager.GoToState (this, "Black", useTransitions);
					VisualStateManager.GoToState (this, "NewCapture", useTransitions);
					break;
				}
			case BoardSpaceState.WhiteNewCapture: {
					VisualStateManager.GoToState (this, "White", useTransitions);
					VisualStateManager.GoToState (this, "NewCapture", useTransitions);
					break;
				}
			}
		}
		private static void _OnSpaceStateChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((BoardSpace)d)._UpdateLayout (true);
		}

		#endregion

		public static readonly DependencyProperty SpaceStateProperty = DependencyProperty.Register ("SpaceState", typeof (BoardSpaceState), typeof (BoardSpace), new PropertyMetadata (BoardSpaceState.Empty, _OnSpaceStateChanged));
		public BoardSpaceState SpaceState
		{
			get
			{
				return (BoardSpaceState)GetValue (SpaceStateProperty);
			}
			set
			{
				SetValue (SpaceStateProperty, value);
			}
		}

		public BoardSpace ()
		{
			DefaultStyleKey = typeof (BoardSpace);
		}
		public override void OnApplyTemplate ()
		{
			base.OnApplyTemplate ();
			_UpdateLayout (false);
		}
	}
}
