namespace Reversi.Core.Algorithms
{
	public struct MiniMaxEvaluation
	{
		public object Space { get; set; }
		public int ScoreValue { get; set; }
	}

	public struct MiniMaxEvaluation<TBoardSpace>
		where TBoardSpace : class
	{
		public TBoardSpace Space { get; set; }
		public int ScoreValue { get; set; }
	}
}
