using System.Diagnostics.Contracts;

namespace Reversi.Core
{
	public static class GamePlayerExtensions
	{
		public static GamePlayer Flip (this GamePlayer player)
		{
			Contract.Assert (player != GamePlayer.InvalidPlayer);
			return player == GamePlayer.Black ? GamePlayer.White : GamePlayer.Black;
		}
		public static GameBoardSpaceState ToBoardSpaceState (this GamePlayer player)
		{
			Contract.Assert (player != GamePlayer.InvalidPlayer);
			return (GameBoardSpaceState)player;
		}
	}
}
