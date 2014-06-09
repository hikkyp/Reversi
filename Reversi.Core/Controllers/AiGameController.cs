using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
		public GamePlayer Player { get; private set; }
		public int SearchDepth { get; set; }

		public AiGameController (Game game, GamePlayer player)
		{
			Game = game;
			Player = player;
			SearchDepth = 2;
		}
		public IList<GameBoardSpace> Move ()
		{
			var movesCount = Game.Moves.Count;
			var move = movesCount < Game.MoveStack.Count
				? Game.MoveStack[movesCount]
				: _Ai.GetBestMove (Game.Board, Player, SearchDepth, null);
			return Game.Move (move);
		}
		public Task<IList<GameBoardSpace>> MoveAsync (CancellationToken? cancellationToken)
		{
			return Task.Run (async () => {
				var movesCount = Game.Moves.Count;
				GameBoardSpace boardSpace;
				if (cancellationToken.HasValue) {
					boardSpace = movesCount < Game.MoveStack.Count
						? Game.MoveStack[movesCount]
						: await Task.Run (() => {
							return _Ai.GetBestMove (Game.Board, Player, SearchDepth, cancellationToken);
						}, cancellationToken.Value);
				} else {
					boardSpace = movesCount < Game.MoveStack.Count
						? Game.MoveStack[movesCount]
						: await Task.Run (() => {
							return _Ai.GetBestMove (Game.Board, Player, SearchDepth, null);
						});
				}
				return await Game.MoveAsync (boardSpace);
			});
		}
	}
}
