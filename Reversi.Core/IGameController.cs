using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reversi.Core
{
	public interface IGameController
	{
		Game Game { get; }

		IList<GameBoardSpace> Move ();
	}
}
