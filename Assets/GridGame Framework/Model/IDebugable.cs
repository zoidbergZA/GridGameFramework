using System.Collections;
using System.Collections.Generic;

namespace GridGame
{
	public interface IDebugable
	{
		string GetLayerName();

		string[,] GetLayerState();
	}
}
