using System.Collections.Generic;

namespace Reversi.Core
{
	public struct GameBoardCaptureDirection
	{
		#region 非表示メンバ

		private static readonly IList<GameBoardCaptureDirection> _ValidDirections = new[] {
			new GameBoardCaptureDirection(-1, 0),
			new GameBoardCaptureDirection(-1, 1),
			new GameBoardCaptureDirection(0, 1),
			new GameBoardCaptureDirection(1, 1),
			new GameBoardCaptureDirection(1, 0),
			new GameBoardCaptureDirection(1, -1),
			new GameBoardCaptureDirection(0, -1),
			new GameBoardCaptureDirection(-1, -1),
		};

		#endregion

		public static IEnumerable<GameBoardCaptureDirection> ValidDirections
		{
			get
			{
				return _ValidDirections;
			}
		}
		public int X { get; private set; }
		public int Y { get; private set; }

		public GameBoardCaptureDirection (int x, int y)
			: this ()
		{
			X = x;
			Y = y;
		}
	}
}
