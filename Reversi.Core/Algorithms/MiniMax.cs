using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Reversi.Core.Algorithms
{
	public sealed class MiniMax<TBoard, TBoardSpace, TPlayer>
		where TBoard : class
		where TBoardSpace : class
	{
		#region 非表示メンバ

		private static readonly Random _Random = new Random ();

		private int _GetMoveValue (TBoard board, TBoardSpace boardSpace, TPlayer player, int searchDepth, int maxValue, CancellationToken? cancellationToken)
		{
			if (cancellationToken.HasValue) {
				cancellationToken.Value.ThrowIfCancellationRequested ();
			}
			board = Move (board, player, boardSpace);
			if (--searchDepth == 0 || IsGameOver (board)) {
				return GetBoardValue (board, player);
			}
			var opponent = FlipPlayer (player);
			var moveEvaluations = searchDepth >= 2
				? _GetMoveEvaluationsParallel (board, opponent, searchDepth, cancellationToken, maxValue)
				: _GetMoveEvaluations (board, opponent, searchDepth, cancellationToken, maxValue);
			return -moveEvaluations.Max (moveEvaluation => moveEvaluation.ScoreValue);
		}
		private IEnumerable<MiniMaxEvaluation<TBoardSpace>> _GetMoveEvaluations (TBoard board, TPlayer player, int searchDepth, CancellationToken? cancellationToken, int maxValue = int.MaxValue)
		{
			var minValue = -int.MaxValue;
			var validMoves = GetValidMoves (board, player).ToArray ();
			var getMoveEvaluation = new Func<TBoardSpace, MiniMaxEvaluation<TBoardSpace>> (move => {
				return new MiniMaxEvaluation<TBoardSpace> {
					Space = move,
					ScoreValue = _GetMoveValue (board, move, player, searchDepth, -minValue, cancellationToken),
				};
			});
			if (validMoves.Any ()) {
				var moveEvaluations = validMoves.Select (move => getMoveEvaluation (move));
				foreach (var moveEvaluation in moveEvaluations) {
					if (moveEvaluation.ScoreValue > maxValue) {
						yield return moveEvaluation;
						break;
					}
					if (moveEvaluation.ScoreValue < minValue) {
						continue;
					}
					minValue = Math.Max (moveEvaluation.ScoreValue, minValue);
					yield return moveEvaluation;
				}
			} else {
				yield return getMoveEvaluation (null);
			}
		}
		private IEnumerable<MiniMaxEvaluation<TBoardSpace>> _GetMoveEvaluationsParallel (TBoard board, TPlayer player, int searchDepth, CancellationToken? cancellationToken, int maxValue = int.MaxValue)
		{
			var minValue = -int.MaxValue;
			var validMoves = GetValidMoves (board, player).ToArray ();
			var getMoveEvaluation = new Func<TBoardSpace, MiniMaxEvaluation<TBoardSpace>> (move => {
				return new MiniMaxEvaluation<TBoardSpace> {
					Space = move,
					ScoreValue = _GetMoveValue (board, move, player, searchDepth, -minValue, cancellationToken),
				};
			});
			var moveEvaluations = new ConcurrentStack<MiniMaxEvaluation<TBoardSpace>> ();
			if (validMoves.Any ()) {
				Parallel.ForEach (validMoves, (validMove, loopState) => {
					var moveEvaluation = getMoveEvaluation (validMove);
					if (moveEvaluation.ScoreValue > maxValue) {
						moveEvaluations.Push (moveEvaluation);
						loopState.Stop ();
					} else if (moveEvaluation.ScoreValue >= minValue) {
						minValue = Math.Max (moveEvaluation.ScoreValue, minValue);
						moveEvaluations.Push (moveEvaluation);
					}
				});
			} else {
				moveEvaluations.Push (getMoveEvaluation (null));
			}
			return moveEvaluations.ToList();
		}

		#endregion

		public Func<TBoard, TPlayer, IEnumerable<TBoardSpace>> GetValidMoves { get; set; }
		public Func<TBoard, TPlayer, int> GetBoardValue { get; set; }
		public Func<TBoard, TPlayer, TBoardSpace, TBoard> Move { get; set; }
		public Func<TBoard, bool> IsGameOver { get; set; }
		public Func<TPlayer, TPlayer> FlipPlayer { get; set; }

		public TBoardSpace GetBestMove (TBoard board, TPlayer player, int searchDepth, CancellationToken? cancellationToken)
		{
			Contract.Requires (GetValidMoves != null);
			Contract.Requires (GetBoardValue != null);
			Contract.Requires (Move != null);
			Contract.Requires (IsGameOver != null);
			Contract.Requires (FlipPlayer != null);
			Contract.Assert (searchDepth > 0);

			var evaluations = searchDepth >= 2
				? _GetMoveEvaluationsParallel (board, player, searchDepth, cancellationToken).ToArray ()
				: _GetMoveEvaluations (board, player, searchDepth, cancellationToken).ToArray ();
			var bestValue = evaluations.Max (evaluation => evaluation.ScoreValue);
			var bestMoves = (
				from evaluation in evaluations
				where evaluation.ScoreValue == bestValue
				select evaluation.Space).ToArray ();
			return bestMoves[_Random.Next (bestMoves.Length)];
		}
	}
}
