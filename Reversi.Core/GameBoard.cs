using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi.Core
{
	public sealed class GameBoard
	{
		#region 非表示メンバ

		private IList<IList<GameBoardSpaceState>> _SpaceStateMatrix;
		private GameBoardSize _Size;

		private GameBoard (IList<IList<GameBoardSpaceState>> spaceStateMatrix, GameBoardSize size)
		{
			_SpaceStateMatrix = spaceStateMatrix;
			_Size = size;
		}
		private void _ResetSpaceStateMatrix ()
		{
			var halfWidth = _Size.Width / 2;
			var halfHeight = _Size.Height / 2;
			_SpaceStateMatrix[halfWidth - 1][halfHeight - 1] = GameBoardSpaceState.Black;
			_SpaceStateMatrix[halfWidth][halfHeight] = GameBoardSpaceState.Black;
			_SpaceStateMatrix[halfWidth - 1][halfHeight] = GameBoardSpaceState.White;
			_SpaceStateMatrix[halfWidth][halfHeight - 1] = GameBoardSpaceState.White;
		}

		private IEnumerable<GameBoardSpaceState> _GetSpaceStatesInDirection (GameBoardSpace startSpace, GameBoardCaptureDirection captureDirection)
		{
			var x = startSpace.X + captureDirection.X;
			var y = startSpace.Y + captureDirection.Y;
			while (Size._Contains (x, y)) {
				yield return _SpaceStateMatrix[x][y];
				x += captureDirection.X;
				y += captureDirection.Y;
			}
		}
		internal bool _GetIsGameOver ()
		{
			return
				!HasEmptySpace ||
				!GameBoardSpace.GetValidMoveSpaces (this, GamePlayer.Black).Any () &&
				!GameBoardSpace.GetValidMoveSpaces (this, GamePlayer.White).Any ();
		}
		internal GameScore _GetScore ()
		{
			var size = Size;
			var blackScore = 0;
			var whiteScore = 0;
			for (var x = 0; x < size.Width; ++x) {
				for (var y = 0; y < size.Height; ++y) {
					switch (_GetSpaceStateAt (x, y)) {
					case GameBoardSpaceState.Black:
						blackScore++;
						break;
					case GameBoardSpaceState.White:
						whiteScore++;
						break;
					}
				}
			}
			return new GameScore (blackScore, whiteScore);
		}
		internal GamePlayer _GetWinner (bool isGameOver)
		{
			var score = _GetScore ();
			return isGameOver && score.BlackScore != score.WhiteScore
				? score.BlackScore > score.WhiteScore
					? GamePlayer.Black
					: GamePlayer.White
				: GamePlayer.InvalidPlayer;
		}
		internal GamePlayer _GetWinner ()
		{
			return _GetWinner (_GetIsGameOver ());
		}
		internal GameBoardSpaceState _GetSpaceStateAt (int x, int y)
		{
			return _SpaceStateMatrix[x][y];
		}
		internal void _SetSpaceStateAt (int x, int y, GameBoardSpaceState spaceState)
		{
			_SpaceStateMatrix[x][y] = spaceState;
		}
		internal GameBoard _Move (GameBoardSpace space, GamePlayer player)
		{
			if (space == null) {
				return this;
			}
			var playerState = player.ToBoardSpaceState ();
			var opponent = player.Flip ();
			var opponentState = opponent.ToBoardSpaceState ();
			var board = Clone ();
			board._SetSpaceStateAt (space.X, space.Y, playerState);
			foreach (var captureDirection in GameBoardCaptureDirection.ValidDirections) {
				if (board.CanCaptureInDirection (space, player, captureDirection)) {
					var x = space.X + captureDirection.X;
					var y = space.Y + captureDirection.Y;
					while (
						board.Size._Contains (x, y) &&
						board._GetSpaceStateAt (x, y) == opponentState) {

						board._SetSpaceStateAt (x, y, playerState);
						x += captureDirection.X;
						y += captureDirection.Y;
					}
				}
			}
			return board;
		}

		#endregion

		public GameBoardSpaceState this[GameBoardSpace space]
		{
			get
			{
				return _GetSpaceStateAt (space.X, space.Y);
			}
			set
			{
				_SetSpaceStateAt (space.X, space.Y, value);
			}
		}
		public GameBoardSize Size { get; private set; }
		public bool HasEmptySpace
		{
			get
			{
				return (_SpaceStateMatrix
					.SelectMany (row => row)
					.Count (spaceState => spaceState != GameBoardSpaceState.Empty) != Size.Area);
			}
		}

		public GameBoard (GameBoardSize size)
		{
			Contract.Requires (size != null);

			Size = size;
			_SpaceStateMatrix = new GameBoardSpaceState[size.Width][];
			foreach (var x in Enumerable.Range (0, size.Width)) {
				_SpaceStateMatrix[x] = new GameBoardSpaceState[size.Height];
			}
			_ResetSpaceStateMatrix ();
		}
		public int GetScoreDifferenceValue (GamePlayer player)
		{
			var score = _GetScore ();
			return player == GamePlayer.Black
				? score.BlackScore - score.WhiteScore
				: score.WhiteScore - score.BlackScore;
		}
		public int GetCornersValue (GamePlayer player)
		{
			var size = Size;
			var playerState = player.ToBoardSpaceState ();
			var cornerValue = size.ShorterSide * 2 + size.LongerSide;
			var cornerStates = new GameBoardSpaceState[] {
				_GetSpaceStateAt(0, 0),
				_GetSpaceStateAt(0, size.Height - 1),
				_GetSpaceStateAt(size.Width - 1, 0),
				_GetSpaceStateAt(size.Width - 1, size.Height - 1),
			};
			return cornerStates
				.Where (cornerState => cornerState != GameBoardSpaceState.Empty)
				.Sum (cornerState => cornerValue * (cornerState == playerState ? 1 : -1));
		}
		public int GetWinValue (GamePlayer player)
		{
			var isGameOver = _GetIsGameOver ();
			return isGameOver
				? int.MaxValue / 2 * (_GetWinner (isGameOver) == player ? 1 : -1)
				: 0;
		}
		public int GetBoardValue (GamePlayer player)
		{
			var scoreDifferenceValue = GetScoreDifferenceValue (player);
			var cornersValue = GetCornersValue (player);
			var winValue = GetWinValue (player);
			return scoreDifferenceValue + cornersValue + winValue;
		}
		public bool CanOccupy (GameBoardSpace space)
		{
			return _GetSpaceStateAt (space.X, space.Y) == GameBoardSpaceState.Empty;
		}
		public bool CanCaptureInDirection (GameBoardSpace space, GamePlayer player, GameBoardCaptureDirection captureDirection)
		{
			var isFirstState = true;
			var playerState = (GameBoardSpaceState)player;
			foreach (var spaceState in _GetSpaceStatesInDirection (space, captureDirection)) {
				if (spaceState == GameBoardSpaceState.Empty) {
					return false;
				}
				if (spaceState == playerState) {
					return !isFirstState;
				}
				isFirstState = false;
			}
			return false;
		}
		public bool CanCapture (GameBoardSpace space, GamePlayer player)
		{
			return GameBoardCaptureDirection
				.ValidDirections
				.Any (captureDirection => CanCaptureInDirection (space, player, captureDirection));
		}
		public GameBoard Clone ()
		{
			var spaceStateMatrix = new GameBoardSpaceState[Size.Width][];
			foreach (var x in Enumerable.Range (0, Size.Width)) {
				spaceStateMatrix[x] = new GameBoardSpaceState[Size.Height];
				_SpaceStateMatrix[x].CopyTo (spaceStateMatrix[x], 0);
			}
			return new GameBoard (spaceStateMatrix, Size);
		}
	}
}
