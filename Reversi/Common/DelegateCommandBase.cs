using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Reversi.Common
{
	public abstract class DelegateCommandBase : ICommand
	{
		#region 非表示メンバ

		private readonly Func<object, Task> _Execute;
		private readonly Func<object, bool> _CanExecute;

		#endregion

		public event EventHandler CanExecuteChanged;

		protected DelegateCommandBase (Action<object> execute, Func<object, bool> canExecute)
		{
			Contract.Requires (execute != null);
			Contract.Requires (canExecute != null);
			_Execute = x => { execute (x); return Task.Delay (0); };
			_CanExecute = canExecute;
		}
		protected DelegateCommandBase (Func<object, Task> execute, Func<object, bool> canExecute)
		{
			Contract.Requires (execute != null);
			Contract.Requires (canExecute != null);
			_Execute = execute;
			_CanExecute = canExecute;
		}
		protected async Task Execute (object parameter)
		{
			await _Execute (parameter);
		}
		async void ICommand.Execute (object parameter)
		{
			await Execute (parameter);
		}
		protected bool CanExecute (object parameter)
		{
			return _CanExecute == null || _CanExecute (parameter);
		}
		bool ICommand.CanExecute (object parameter)
		{
			return CanExecute (parameter);

		}
		protected virtual void OnCanExecuteChanged ()
		{
			var eventHandler = CanExecuteChanged;
			if (eventHandler != null) {
				eventHandler (this, EventArgs.Empty);
			}
		}
		[SuppressMessage ("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
		public void RaiseCanExecuteChanged ()
		{
			OnCanExecuteChanged ();
		}
	}
}
