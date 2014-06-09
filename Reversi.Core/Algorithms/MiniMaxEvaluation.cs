namespace Reversi.Core.Algorithms
{
	public struct MiniMaxEvaluation<TBoardSpace>
		where TBoardSpace : class
	{
		public TBoardSpace Space { get; set; }
		public int ScoreValue { get; set; }
	}
}
