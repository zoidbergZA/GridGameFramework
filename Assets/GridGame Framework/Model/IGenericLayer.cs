using System.Collections;
using System.Collections.Generic;

namespace GridGame
{
	public interface IGenericLayer
	{
		string GetLayerName();
		bool GetIsDebugable();
		string[,] GetDebugState();
	}
}
