using System.Collections.Generic;
using System.Linq;

namespace Reversi.Core
{
	using Algorithms;

	public sealed class GameBoardSpace
	{
		#region 非表示メンバ

		private static readonly Dictionary<GameBoardSize, IEnumerable<GameBoardSpace>> _AllSpaces = new Dictionary<GameBoardSize, IEnumerable<GameBoardSpace>> ();

		#endregion

		public int X { get; private set; }
		public int Y { get; private set; }

		public GameBoardSpace (int x, int y)
		{
			X = x;
			Y = y;
		}
		public static IEnumerable<GameBoardSpace> GetMoveSpaces (GameBoardSize boardSize)
		{
			if (!_AllSpaces.ContainsKey (boardSize)) {
				_AllSpaces[boardSize] =
					from x in Enumerable.Range (0, boardSize.Width)
					from y in Enumerable.Range (0, boardSize.Height)
					select new GameBoardSpace (x, y);
			}
			return _AllSpaces[boardSize];
		}
		public static IEnumerable<GameBoardSpace> GetValidMoveSpaces (GameBoard board, GamePlayer player)
		{
			return GetMoveSpaces (board.Size)
				.Where (boardSpace =>
					board.Size.Contains (boardSpace) &&
					board.CanOccupy (boardSpace) &&
					board.CanCapture (boardSpace, player));
		}
		public override string ToString ()
		{
			return string.Format ("({0},{1})", X, Y);
		}
		public override bool Equals (object obj)
		{
			var other = obj as GameBoardSpace;
			if (other == null) {
				return false;
			}
			return X == other.X && Y == other.Y;
		}
		public override int GetHashCode ()
		{
			return HashCombinator.Combine (X.GetHashCode (), Y.GetHashCode ());
		}
	}
}
