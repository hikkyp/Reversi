using System;
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
		private static readonly string _BlackSpaceSymbol = "●";
		private static readonly string _WhiteSpaceSymbol = "○";
		private static readonly string _EmptySpaceSymbol = "　";

		private static void _PrintBoard (Game game)
		{
			var board = game.Board;
			var boardSize = board.Size;
			var printBuffer = "";

			printBuffer += "┏";
			for (var x = 0; x < boardSize.Width; ++x) {
				printBuffer += "━";
			}
			printBuffer += "┓";
			printBuffer += "\n";
			for (var x = 0; x < boardSize.Width; ++x) {
				printBuffer += "┃";
				for (var y = 0; y < boardSize.Height; ++y) {
					switch (board[new GameBoardSpace (x, y)]) {
					case GameBoardSpaceState.Black:
						printBuffer += _BlackSpaceSymbol;
						break;
					case GameBoardSpaceState.White:
						printBuffer += _WhiteSpaceSymbol;
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
			var random = new Random ();
			while (true) {
				var game = new Game ();
				var gameController1 = new AiGameController (game) { SearchDepth = searchDepths[random.Next (searchDepths.Count ())] };
				var gameController2 = new AiGameController (game) { SearchDepth = searchDepths[random.Next (searchDepths.Count ())] };
				var turnFlag = false;
				while (!game.IsOver) {
					if (turnFlag) {
						gameController2.Move ();
					} else {
						gameController1.Move ();
					}
					_PrintBoard (game);
				}
				_PrintScore (game);
			}
		}
	}

}
