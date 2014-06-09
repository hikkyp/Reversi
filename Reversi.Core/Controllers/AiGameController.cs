using System.Collections.Generic;

namespace Reversi.Core.Controllers
{
	using Algorithms;

	public sealed class AiGameController : IGameController
	{
		#region 非表示メンバ

		private static readonly MiniMax<GameBoard, GameBoardSpace, GamePlayer> _Ai = new MiniMax<GameBoard, GameBoardSpace, GamePlayer> {
			GetValidMoves = (board, player) => GameBoardSpace.GetValidMoveSpaces (board, player),
			GetBoardValue = (board, player) => board.GetBoardValue (player),
			Move = (board, player, space) => board._Move (space, player),
			IsGameOver = (board) => board._GetIsGameOver (),
			FlipPlayer = (player) => player.Flip (),
		};

		#endregion

		public Game Game { get; private set; }
		public int SearchDepth { get; set; }

		public AiGameController (Game game)
		{
			Game = game;
			SearchDepth = 2;
		}
		public IList<GameBoardSpace> Move ()
		{
			var movesCount = Game.Moves.Count;
			var move = movesCount < Game.MoveStack.Count
				? Game.MoveStack[movesCount]
				: _Ai.GetBestMove (Game.Board, Game.CurrentPlayer, SearchDepth, null);
			return Game.Move (move);
		}
	}
}
