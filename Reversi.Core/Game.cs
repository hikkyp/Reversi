using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi.Core
{
	public sealed class Game
	{
		#region 非表示メンバ

		private static readonly object _SyncObject = new object ();
		private List<GameBoardSpace> _Moves = new List<GameBoardSpace> ();
		private List<GameBoardSpace> _MoveStack = new List<GameBoardSpace> ();

		private void _SyncMoveStack (GameBoardSpace boardSpace)
		{
			var movesCount = _Moves.Count;
			if (movesCount > _MoveStack.Count) {
				_MoveStack.Add (boardSpace);
			} else {
				var stackMove = _MoveStack[movesCount - 1];
				if ((stackMove == null && stackMove != boardSpace) ||
					(stackMove != null && !stackMove.Equals (boardSpace))) {

					_MoveStack = new List<GameBoardSpace> (_Moves);
				}
			}
		}
		private IList<GameBoardSpace> _Move (GameBoardSpace boardSpace)
		{
			var changedBoardSpaces = new List<GameBoardSpace> ();
			if (boardSpace != null) {
				var board = Board._Move (boardSpace, CurrentPlayer);
				var boardSize = board.Size;
				for (int x = 0; x < boardSize.Width; ++x) {
					for (int y = 0; y < boardSize.Height; ++y) {
						if (board._GetSpaceStateAt (x, y) != Board._GetSpaceStateAt (x, y)) {
							changedBoardSpaces.Add (new GameBoardSpace (x, y));
						}
					}
				}
				changedBoardSpaces.Remove (boardSpace);
				changedBoardSpaces.Insert (0, boardSpace);
				Board = board;
			}
			_Moves.Add (boardSpace);
			return changedBoardSpaces;
		}
		private IList<GameBoardSpace> _Reset (int turnCount)
		{
			Contract.Assert (turnCount >= 0 && turnCount <= MoveStack.Count);
			Contract.Assert (turnCount != Moves.Count);

			IList<GameBoardSpace> moves;
			if (turnCount < Moves.Count) {
				Board = new GameBoard (Board.Size);
				moves = _Moves.Take (turnCount).ToList ();
				_Moves.Clear ();
			} else {
				moves = _MoveStack
					.Skip (_Moves.Count)
					.Take (turnCount - _Moves.Count).ToList ();
			}
			return Move (moves);
		}

		#endregion

		public GameBoard Board { get; private set; }
		public IList<GameBoardSpace> Moves { get { return _Moves; } }
		public IList<GameBoardSpace> MoveStack { get { return _MoveStack; } }
		public GamePlayer CurrentPlayer
		{
			get
			{
				return Moves.Count % 2 == 0 ? GamePlayer.Black : GamePlayer.White;
			}
		}
		public GamePlayer CurrentOpponent
		{
			get
			{
				return Moves.Count % 2 == 0 ? GamePlayer.White : GamePlayer.Black;
			}
		}
		public GameScore Score { get { return Board._GetScore (); } }
		public bool IsOver { get { return Board._GetIsGameOver (); } }

		public Game (GameBoardSize boardSize)
		{
			Board = new GameBoard (boardSize);
		}
		public Game () : this (new GameBoardSize ()) { }
		public IList<GameBoardSpace> Move (GameBoardSpace boardSpace)
		{
			var changedBoardSpaces = _Move (boardSpace);
			_SyncMoveStack (boardSpace);
			return changedBoardSpaces;
		}
		public IList<GameBoardSpace> Move (IEnumerable<GameBoardSpace> boardSpaces)
		{
			var boardSpacesList = boardSpaces.ToList ();
			if (boardSpacesList.Count == 0) {
				return new List<GameBoardSpace> ();
			}
			foreach (var boardSpace in boardSpacesList.Take (boardSpacesList.Count - 1)) {
				Move (boardSpace);
			}
			return Move (boardSpacesList.Last ());
		}
		public IList<GameBoardSpace> Pass ()
		{
			return Move ((GameBoardSpace)null);
		}
		public IList<GameBoardSpace> Undo ()
		{
			return _Reset (_Moves.Count - 1);
		}
		public IList<GameBoardSpace> Redo ()
		{
			return _Reset (_Moves.Count + 1);
		}
	}
}
