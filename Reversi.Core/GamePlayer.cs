namespace Reversi.Core
{
	public enum GamePlayer
	{
		InvalidPlayer = (int)GameBoardSpaceState.Empty,
		Black = (int)GameBoardSpaceState.Black,
		White = (int)GameBoardSpaceState.White,
	}
}
