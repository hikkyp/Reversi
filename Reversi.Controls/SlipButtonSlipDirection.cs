namespace Reversi.Controls
{
	/// <summary>
	/// スリップボタンにカーソルが重なったときのシンボルの現れ方を決めます。
	/// </summary>
	public enum SlipButtonSlipDirection
	{
		/// <summary>
		/// カーソルが重なると上からシンボルが現れます。
		/// </summary>
		TopToBottom,

		/// <summary>
		/// カーソルが重なると左からシンボルが現れます。
		/// </summary>
		LeftToRight,

		/// <summary>
		/// カーソルが重なると下からシンボルが現れます。
		/// </summary>
		BottomToTop,

		/// <summary>
		/// カーソルが重なると右からシンボルが現れます。
		/// </summary>
		RightToLeft,
	}
}
