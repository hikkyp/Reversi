using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Reversi.Core
{
	public interface IGameController
	{
		Game Game { get; }
		GamePlayer Player { get; }

		IList<GameBoardSpace> Move ();
		Task<IList<GameBoardSpace>> MoveAsync (CancellationToken? cancellationToken);
	}
}
