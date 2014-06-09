namespace Reversi.Core
{
	public struct GameScore
	{
		public int BlackScore { get; private set; }
		public int WhiteScore { get; private set; }
		public int TotalScore { get { return BlackScore + WhiteScore; } }

		public GameScore (int blackScore, int whiteScore)
			: this ()
		{
			BlackScore = blackScore;
			WhiteScore = whiteScore;
		}
		public int GetScore (GamePlayer player)
		{
			return player == GamePlayer.Black
				? BlackScore
				: player == GamePlayer.White
					? WhiteScore
					: TotalScore;
		}
	}
}
