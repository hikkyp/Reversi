using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Reversi.Core.Algorithms
{
	public sealed class MiniMax
	{
		#region 非表示メンバ

		private static readonly Random _Random = new Random ();
		private int _MaxSearchRecursion;
		private bool _StartEvaluationSide;
		private Func<object, bool, IEnumerable<object>> _GetValidMoves;
		private Func<object, bool, int> _GetBoardValue;
		private Func<object, object, bool, object> _Move;
		private Func<object, bool> _IsGameOver;

		private int _GetMoveValueCancellable (object board, object boardSpace, int searchRecursion, int maxValue, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested ();
			var evaluatingSide = _StartEvaluationSide;
			if (searchRecursion % 2 != 0) {
				evaluatingSide = !evaluatingSide;
			}
			board = _Move (board, boardSpace, evaluatingSide);
			return (++searchRecursion == _MaxSearchRecursion || _IsGameOver (board))
				? _GetBoardValue (board, !evaluatingSide)
				: -_GetMoveEvaluationsCancellable (board, searchRecursion, maxValue, cancellationToken)
					.Max (moveEvaluation => moveEvaluation.ScoreValue);
		}
		private int _GetMoveValue (object board, object boardSpace, int searchRecursion, int maxValue)
		{
			var evaluatingSide = _StartEvaluationSide;
			if (searchRecursion % 2 != 0) {
				evaluatingSide = !evaluatingSide;
			}
			board = _Move (board, boardSpace, evaluatingSide);
			return (++searchRecursion == _MaxSearchRecursion || _IsGameOver (board))
				? _GetBoardValue (board, !evaluatingSide)
				: -_GetMoveEvaluations (board, searchRecursion, maxValue)
					.Max (moveEvaluation => moveEvaluation.ScoreValue);
		}
		private IEnumerable<MiniMaxEvaluation> _GetMoveEvaluationsCancellable (object board, int searchRecursion, int maxValue, CancellationToken cancellationToken)
		{
			var minValue = -int.MaxValue;
			var evaluatingSide = _StartEvaluationSide;
			if (searchRecursion % 2 != 0) {
				evaluatingSide = !evaluatingSide;
			}
			var validMoves = _GetValidMoves (board, evaluatingSide).ToArray ();
			var getMoveEvaluation = new Func<object, MiniMaxEvaluation> (move => new MiniMaxEvaluation {
				Space = move,
				ScoreValue = _GetMoveValueCancellable (board, move, searchRecursion, -minValue, cancellationToken),
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
		private IEnumerable<MiniMaxEvaluation> _GetMoveEvaluations (object board, int searchRecursion, int maxValue)
		{
			var minValue = -int.MaxValue;
			var evaluatingSide = _StartEvaluationSide;
			if (searchRecursion % 2 != 0) {
				evaluatingSide = !evaluatingSide;
			}
			var validMoves = _GetValidMoves (board, evaluatingSide).ToArray ();
			var getMoveEvaluation = new Func<object, MiniMaxEvaluation> (move => new MiniMaxEvaluation {
				Space = move,
				ScoreValue = _GetMoveValue (board, move, searchRecursion, -minValue),
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

		#endregion

		public MiniMax (int maxSearchRecursion, Func<object, bool, IEnumerable<object>> getValidMoves, Func<object, bool, int> getBoardValue, Func<object, object, bool, object> move, Func<object, bool> isGameOver)
		{
			Contract.Assert (maxSearchRecursion > 0);
			Contract.Assert (getValidMoves != null);
			Contract.Assert (getBoardValue != null);
			Contract.Assert (move != null);
			Contract.Assert (isGameOver != null);
			_MaxSearchRecursion = maxSearchRecursion;
			_GetValidMoves = getValidMoves;
			_GetBoardValue = getBoardValue;
			_Move = move;
			_IsGameOver = isGameOver;
		}
		public object GetBestMoveCancellable (object board, bool evaluationSide, CancellationToken cancellationToken)
		{
			_StartEvaluationSide = evaluationSide;
			var evaluations = _GetMoveEvaluationsCancellable (board, 0, int.MaxValue, cancellationToken).ToArray ();
			var bestValue = evaluations.Max (evaluation => evaluation.ScoreValue);
			var bestMoves = (
				from evaluation in evaluations
				where evaluation.ScoreValue == bestValue
				select evaluation.Space).ToArray ();
			return bestMoves[_Random.Next (bestMoves.Length)];
		}
		public object GetBestMove (object board, bool evaluationSide)
		{
			_StartEvaluationSide = evaluationSide;
			var evaluations = _GetMoveEvaluations (board, 0, int.MaxValue).ToArray ();
			var bestValue = evaluations.Max (evaluation => evaluation.ScoreValue);
			var bestMoves = (
				from evaluation in evaluations
				where evaluation.ScoreValue == bestValue
				select evaluation.Space).ToArray ();
			return bestMoves[_Random.Next (bestMoves.Length)];
		}
	}

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
			return moveEvaluations.ToList ();
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
