using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Reversi.ViewModels
{
	using Common;

	class GameClockViewModel : BindableBase
	{
		#region 非表示メンバ

		private GameViewModel _GameViewModel;
		private Stopwatch _WhiteMoveTimeStopwatch = new Stopwatch ();
		private Stopwatch _BlackMoveTimeStopwatch = new Stopwatch ();
		private Stopwatch _WhiteTotalTimeStopwatch = new Stopwatch ();
		private Stopwatch _BlackTotalTimeStopwatch = new Stopwatch ();
		private DelegateCommand _StartCommand;
		private DelegateCommand _PauseCommand;
		private DispatcherTimer _Timer = new DispatcherTimer {
			Interval = TimeSpan.FromSeconds (1),
		};
		private TimeSpan _BlackMoveTimeSpan;
		private TimeSpan _WhiteMoveTimeSpan;

		private void _Timer_OnTick (object sender, EventArgs e)
		{
			throw new NotImplementedException ();
		}
		private void _GameViewModel_OnPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			switch (e.PropertyName) {
			case "CurrentPlayer": {
					break;
				}
			case "IsGameOver": {
					break;
				}
			}
		}

		private Task _Pause ()
		{
			throw new NotImplementedException ();
		}

		#endregion

		public GameViewModel GameViewModel
		{
			get
			{
				return _GameViewModel;
			}
			set
			{
				if (value != null && value != _GameViewModel) {
					if (_GameViewModel != null) {
						_GameViewModel.PropertyChanged -= _GameViewModel_OnPropertyChanged;
					}
					_GameViewModel = value;
					_GameViewModel.PropertyChanged += _GameViewModel_OnPropertyChanged;
				}
			}
		}
		public DelegateCommand StartCommand
		{
			get
			{
				if (_StartCommand == null) {
					_StartCommand = new DelegateCommand (Start, () => IsPaused);
				}
				return _StartCommand;
			}
		}
		public DelegateCommand PauseCommand
		{
			get
			{
				if (_PauseCommand == null) {
					_PauseCommand = DelegateCommand.FromAsyncHandler (_Pause, () => !IsPaused);
				}
				return _PauseCommand;
			}
		}
		public TimeSpan BlackMoveTimeSpan
		{
			get
			{
				var value = _BlackMoveTimeStopwatch.Elapsed + _BlackMoveTimeSpan;
				if (_BlackMoveTimeSpan.Ticks > 0) {
					_BlackMoveTimeSpan = new TimeSpan ();
				}
				return value;
			}
			set
			{
				_BlackMoveTimeSpan = value;
			}
		}
		public TimeSpan WhiteMoveTimeSpan
		{
			get
			{
				var value = _WhiteMoveTimeStopwatch.Elapsed + _WhiteMoveTimeSpan;
				if (_WhiteMoveTimeSpan.Ticks > 0) {
					_WhiteMoveTimeSpan = new TimeSpan ();
				}
				return value;
			}
			set
			{
				_WhiteMoveTimeSpan = value;
			}
		}
		public bool IsPaused
		{
			get
			{
				return !_Timer.IsEnabled;
			}
		}

		public GameClockViewModel (GameViewModel gameViewModel)
		{
			_GameViewModel = gameViewModel;
			_Timer.Tick += _Timer_OnTick;
			if (gameViewModel != null) {
				Start ();
			}
		}
		public GameClockViewModel ()
			: this (null)
		{
		}
		public void Start ()
		{
		}
		public void Stop ()
		{

		}

	}
}
