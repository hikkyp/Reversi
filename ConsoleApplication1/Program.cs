using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reversi.Core;
using Reversi.Core.Controllers;

namespace ConsoleApplication1
{
	public static class Program
	{
		private static readonly string _BlackSpaceSymbol = "○";
		private static readonly string _WhiteSpaceSymbol = "●";
		private static readonly string _BlackSpaceSymbolNewMove = "◇";
		private static readonly string _WhiteSpaceSymbolNewMove = "◆";
		private static readonly string _EmptySpaceSymbol = "　";

		private static void _PrintBoard (Game game, IList<GameBoardSpace> moves)
		{
			var board = game.Board;
			var boardSize = board.Size;
			var printBuffer = "";

			GameBoardSpace newMove;
			if (moves != null) {
				newMove = moves.First ();
			} else {
				newMove = new GameBoardSpace (int.MaxValue, int.MaxValue);
			}

			printBuffer += "┏";
			for (var x = 0; x < boardSize.Width; ++x) {
				printBuffer += "━";
			}
			printBuffer += "┓";
			printBuffer += "\n";
			for (var y = 0; y < boardSize.Height; ++y) {
				printBuffer += "┃";
				for (var x = 0; x < boardSize.Width; ++x) {
					var isNewMove = newMove.X == x && newMove.Y == y;
					switch (board[new GameBoardSpace (x, y)]) {
					case GameBoardSpaceState.Black:
						printBuffer += isNewMove ? _BlackSpaceSymbolNewMove : _BlackSpaceSymbol;
						break;
					case GameBoardSpaceState.White:
						printBuffer += isNewMove ? _WhiteSpaceSymbolNewMove : _WhiteSpaceSymbol;
						break;
					case GameBoardSpaceState.Empty:
						printBuffer += _EmptySpaceSymbol;
						break;
					}
				}
				printBuffer += "┃";
				printBuffer += "\n";
			}
			printBuffer += "┗";
			for (var x = 0; x < boardSize.Width; ++x) {
				printBuffer += "━";
			}
			printBuffer += "┛";
			Console.WriteLine (printBuffer);
		}
		private static void _PrintScore (Game game)
		{
			var score = game.Score;
			var printBuffer = "";
			printBuffer += "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n";
			printBuffer += string.Format ("black {0} white {1}\n", score.BlackScore, score.WhiteScore);
			printBuffer += "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n";
			Console.WriteLine (printBuffer);
		}

		static void Main (string[] args)
		{
			var searchDepths = new int[] { 1, 2, 3, 4, 5 };
			var boardWidth = new int[] { 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32, 34, 36 };
			var boardHeight = new int[] { 4, 6, 8, 10, 12, 14, 16, 18, 20, 22 };
			var random = new Random ();
			while (true) {
				var game = new Game (new GameBoardSize (
					boardWidth[random.Next (boardWidth.Count ())],
					boardHeight[random.Next (boardHeight.Count ())]
					));
				var gameController1 = new AiGameController (game, GamePlayer.Black) { SearchDepth = searchDepths[random.Next (searchDepths.Count ())] };
				var gameController2 = new AiGameController (game, GamePlayer.White) { SearchDepth = searchDepths[random.Next (searchDepths.Count ())] };
				while (!game.IsOver) {
					Task<IList<GameBoardSpace>> thinkTask;
					var thinkTimer = Stopwatch.StartNew ();
					int thinkDepth;
					var player = game.CurrentPlayer;
					if (player == GamePlayer.Black) {
						thinkDepth = gameController1.SearchDepth;
						thinkTask = gameController1.MoveAsync (null);
					} else {
						thinkDepth = gameController2.SearchDepth;
						thinkTask = gameController2.MoveAsync (null);
					}
					Console.Write ("{0} thinking", player);
					while (!thinkTask.Wait (100)) {
						Console.Write (".");
					}
					var thinkSpan = thinkTimer.ElapsedMilliseconds;
					Console.WriteLine (" think time {0} ms (max depth {1})", thinkSpan, thinkDepth);
					_PrintBoard (game, thinkTask.Result);
				}
				_PrintScore (game);
			}
		}
	}

}
