using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Reversi.Controls
{
	public class SlipButton : Button
	{
		#region 非表示メンバ

		private Canvas _Container;
		private Grid _Caption;
		private Grid _CaptionGroup;
		private Border _CaptionImage;
		private Grid _CaptionLabel;

		static SlipButton ()
		{
			DefaultStyleKeyProperty.OverrideMetadata (typeof (SlipButton), new FrameworkPropertyMetadata (typeof (SlipButton)));
		}
		private void _UpdateLayout ()
		{
			var slipDirection = SlipDirection;

			// _Image と _Label のグリッド内配置をリセットして、
			// グリッドの行・列定義をクリア
			Grid.SetRow (_CaptionImage, 0);
			Grid.SetColumn (_CaptionImage, 0);
			Grid.SetRow (_CaptionLabel, 0);
			Grid.SetColumn (_CaptionImage, 0);
			_Caption.RowDefinitions.Clear ();
			_Caption.ColumnDefinitions.Clear ();

			// 行・列定義を構成して、
			// _Image と _Label のグリッド内配置を決定する
			switch (slipDirection) {
			case SlipButtonSlipDirection.TopToBottom: {
					_Caption.RowDefinitions.Add (new RowDefinition ());
					_Caption.RowDefinitions.Add (new RowDefinition ());
					Grid.SetRow (_CaptionLabel, 1);
					Grid.SetRow (_CaptionImage, 0);
					break;
				}
			case SlipButtonSlipDirection.LeftToRight: {
					_Caption.ColumnDefinitions.Add (new ColumnDefinition ());
					_Caption.ColumnDefinitions.Add (new ColumnDefinition ());
					Grid.SetColumn (_CaptionLabel, 1);
					Grid.SetColumn (_CaptionImage, 0);
					break;
				}
			case SlipButtonSlipDirection.BottomToTop: {
					_Caption.RowDefinitions.Add (new RowDefinition ());
					_Caption.RowDefinitions.Add (new RowDefinition ());
					Grid.SetRow (_CaptionLabel, 0);
					Grid.SetRow (_CaptionImage, 1);
					break;
				}
			case SlipButtonSlipDirection.RightToLeft: {
					_Caption.ColumnDefinitions.Add (new ColumnDefinition ());
					_Caption.ColumnDefinitions.Add (new ColumnDefinition ());
					Grid.SetColumn (_CaptionLabel, 0);
					Grid.SetColumn (_CaptionImage, 1);
					break;
				}
			}

			// コンテナの各ビジュアルステートグループに対して、
			// アニメーションを構成する
			var captionLeft = -ActualWidth;
			var captionTop = -ActualHeight;
			foreach (var visualStateGroup in (ObservableCollection<VisualStateGroup>)_Container.GetValue (VisualStateManager.VisualStateGroupsProperty)) {
				if (visualStateGroup.Name == "CommonStates") {
					foreach (var visualStateObject in visualStateGroup.States) {
						var visualState = (VisualState)visualStateObject;
						var storyboard = visualState.Storyboard;
						foreach (var animation in storyboard.Children) {
							switch (animation.Name) {

							#region NormalCaptionHorizontalAnimation

							case "NormalCaptionHorizontalSlipAniamtion": {
									var doubleAnimation = (DoubleAnimationUsingKeyFrames)animation;
									switch (slipDirection) {
									case SlipButtonSlipDirection.TopToBottom: {
											doubleAnimation.KeyFrames.Clear ();
											break;
										}
									case SlipButtonSlipDirection.LeftToRight: {
											doubleAnimation.KeyFrames.Clear ();
											doubleAnimation.KeyFrames.Add (new EasingDoubleKeyFrame (captionLeft, KeyTime.FromPercent (1)));
											_Caption.BeginAnimation (Canvas.LeftProperty, doubleAnimation);
											break;
										}
									case SlipButtonSlipDirection.BottomToTop: {
											doubleAnimation.KeyFrames.Clear ();
											break;
										}
									case SlipButtonSlipDirection.RightToLeft: {
											doubleAnimation.KeyFrames.Clear ();
											doubleAnimation.KeyFrames.Add (new EasingDoubleKeyFrame (0, KeyTime.FromPercent (1)));
											_Caption.BeginAnimation (Canvas.LeftProperty, doubleAnimation);
											break;
										}
									}
									break;
								}

							#endregion

							#region NormalCaptionVerticalSlipAniamtion

							case "NormalCaptionVerticalSlipAniamtion": {
									var doubleAnimation = (DoubleAnimationUsingKeyFrames)animation;
									switch (slipDirection) {
									case SlipButtonSlipDirection.TopToBottom: {
											doubleAnimation.KeyFrames.Clear ();
											doubleAnimation.KeyFrames.Add (new EasingDoubleKeyFrame (captionTop, KeyTime.FromPercent (1)));
											_Caption.BeginAnimation (Canvas.TopProperty, doubleAnimation);
											break;
										}
									case SlipButtonSlipDirection.LeftToRight: {
											doubleAnimation.KeyFrames.Clear ();
											break;
										}
									case SlipButtonSlipDirection.BottomToTop: {
											doubleAnimation.KeyFrames.Clear ();
											doubleAnimation.KeyFrames.Add (new EasingDoubleKeyFrame (0, KeyTime.FromPercent (1)));
											_Caption.BeginAnimation (Canvas.TopProperty, doubleAnimation);
											break;
										}
									case SlipButtonSlipDirection.RightToLeft: {
											doubleAnimation.KeyFrames.Clear ();
											break;
										}
									}
									break;
								}

							#endregion

							#region MouseOverCaptionHorizontalSlipAniamtion

							case "MouseOverCaptionHorizontalSlipAniamtion": {
									var doubleAnimation = (DoubleAnimationUsingKeyFrames)animation;
									switch (slipDirection) {
									case SlipButtonSlipDirection.TopToBottom: {
											doubleAnimation.KeyFrames.Clear ();
											Canvas.SetLeft (_Caption, 0);
											break;
										}
									case SlipButtonSlipDirection.LeftToRight: {
											doubleAnimation.KeyFrames.Clear ();
											doubleAnimation.KeyFrames.Add (new EasingDoubleKeyFrame (0, KeyTime.FromPercent (1)));
											Canvas.SetLeft (_Caption, 0);
											break;
										}
									case SlipButtonSlipDirection.BottomToTop: {
											doubleAnimation.KeyFrames.Clear ();
											Canvas.SetLeft (_Caption, 0);
											break;
										}
									case SlipButtonSlipDirection.RightToLeft: {
											doubleAnimation.KeyFrames.Clear ();
											doubleAnimation.KeyFrames.Add (new EasingDoubleKeyFrame (captionLeft, KeyTime.FromPercent (1)));
											Canvas.SetLeft (_Caption, captionLeft);
											break;
										}
									}
									break;
								}

							#endregion

							#region MouseOverCaptionVerticalSlipAniamtion

							case "MouseOverCaptionVerticalSlipAniamtion": {
									var doubleAnimation = (DoubleAnimationUsingKeyFrames)animation;
									switch (slipDirection) {
									case SlipButtonSlipDirection.TopToBottom: {
											doubleAnimation.KeyFrames.Clear ();
											doubleAnimation.KeyFrames.Add (new EasingDoubleKeyFrame (0, KeyTime.FromPercent (1)));
											Canvas.SetTop (_Caption, 0);
											break;
										}
									case SlipButtonSlipDirection.LeftToRight: {
											doubleAnimation.KeyFrames.Clear ();
											Canvas.SetTop (_Caption, 0);
											break;
										}
									case SlipButtonSlipDirection.BottomToTop: {
											doubleAnimation.KeyFrames.Clear ();
											doubleAnimation.KeyFrames.Add (new EasingDoubleKeyFrame (captionTop, KeyTime.FromPercent (1)));
											Canvas.SetTop (_Caption, captionTop);
											break;
										}
									case SlipButtonSlipDirection.RightToLeft: {
											doubleAnimation.KeyFrames.Clear ();
											Canvas.SetTop (_Caption, 0);
											break;
										}
									}
									break;
								}

							#endregion
							}
						}
					}
				}
			}
		}
		private void _Container_OnSizeChanged (object sender, SizeChangedEventArgs e)
		{
			_UpdateLayout ();
		}

		#endregion

		public static readonly DependencyProperty SymbolSizeProperty = DependencyProperty.Register ("SymbolSize", typeof (double), typeof (SlipButton));
		public static readonly DependencyProperty SymbolProperty = DependencyProperty.Register ("Symbol", typeof (SlipButtonSymbol), typeof (SlipButton));
		public static readonly DependencyProperty SlipDirectionProperty = DependencyProperty.Register ("SlipDirection", typeof (SlipButtonSlipDirection), typeof (SlipButton), new UIPropertyMetadata (SlipButtonSlipDirection.LeftToRight));
		public double SymbolSize
		{
			get
			{
				return (double)GetValue (SymbolSizeProperty);
			}
			set
			{
				SetValue (SymbolSizeProperty, value);
			}
		}
		public SlipButtonSymbol Symbol
		{
			get
			{
				return (SlipButtonSymbol)GetValue (SymbolProperty);
			}
			set
			{
				SetValue (SymbolProperty, value);
			}
		}
		public SlipButtonSlipDirection SlipDirection
		{
			get
			{
				return (SlipButtonSlipDirection)GetValue (SlipDirectionProperty);
			}
			set
			{
				SetValue (SlipDirectionProperty, value);
				_UpdateLayout ();
			}
		}

		public SlipButton ()
		{
		}
		public override void OnApplyTemplate ()
		{
			if (_Caption == null) {
				_Caption = (Grid)GetTemplateChild ("Caption");
			}
			if (_CaptionGroup == null) {
				_CaptionGroup = (Grid)GetTemplateChild ("CaptionGroup");
			}
			if (_CaptionLabel == null) {
				_CaptionLabel = (Grid)GetTemplateChild ("CaptionLabel");
			}
			if (_CaptionImage == null) {
				_CaptionImage = (Border)GetTemplateChild ("CaptionImage");
			}
			if (_Container == null) {
				_Container = (Canvas)GetTemplateChild ("Container");
				_Container.SizeChanged += _Container_OnSizeChanged;
			}
		}
	}
}
