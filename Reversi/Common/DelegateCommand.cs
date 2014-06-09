using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace Reversi.Common
{
	public class DelegateCommand<T> : DelegateCommandBase
	{
		#region 非表示メンバ

		private DelegateCommand (Func<T, Task> execute, Func<T, bool> canExecute)
			: base (x => execute ((T)x), x => canExecute ((T)x))
		{
			Contract.Requires (execute != null);
			Contract.Requires (canExecute != null);
		}
		private DelegateCommand (Func<T, Task> execute)
			: this (execute, x => true)
		{
		}

		#endregion

		public DelegateCommand (Action<T> execute, Func<T, bool> canExecute)
			: base (x => execute ((T)x), x => canExecute ((T)x))
		{
			Contract.Requires (execute != null);
			Contract.Requires (canExecute != null);
		}
		public DelegateCommand (Action<T> execute)
			: this (execute, x => true)
		{
		}
		public static DelegateCommand<T> FromAsyncHandler (Func<T, Task> execute)
		{
			return new DelegateCommand<T> (execute);
		}
		public static DelegateCommand<T> FromAsyncHandler (Func<T, Task> execute, Func<T, bool> canExecute)
		{
			return new DelegateCommand<T> (execute, canExecute);
		}
		public async Task Execute (T parameter)
		{
			await base.Execute (parameter);
		}
		public bool CanExecute (T parameter)
		{
			return base.CanExecute (parameter);
		}
	}

	public class DelegateCommand : DelegateCommandBase
	{
		#region 非表示メンバ

		private DelegateCommand (Func<Task> execute)
			: this (execute, () => true)
		{
		}
		private DelegateCommand (Func<Task> execute, Func<bool> canExecute)
			: base (x => execute (), x => canExecute ())
		{
			Contract.Requires (execute != null);
			Contract.Requires (canExecute != null);
		}

		#endregion

		public DelegateCommand (Action execute, Func<bool> canExecute)
			: base (x => execute (), x => canExecute ())
		{
			Contract.Requires (execute != null);
			Contract.Requires (canExecute != null);
		}
		public DelegateCommand (Action execute)
			: this (execute, () => true)
		{
		}
		public static DelegateCommand FromAsyncHandler (Func<Task> execute)
		{
			return new DelegateCommand (execute);
		}
		public static DelegateCommand FromAsyncHandler (Func<Task> execute, Func<bool> canExecute)
		{
			return new DelegateCommand (execute, canExecute);
		}
		public async Task Execute ()
		{
			await Execute (null);
		}
		public bool CanExecute ()
		{
			return CanExecute (null);
		}
	}
}
