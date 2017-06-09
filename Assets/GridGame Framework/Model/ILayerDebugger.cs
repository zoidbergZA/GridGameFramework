using System.Collections;
using System.Collections.Generic;

namespace GridGame
{
	public abstract class ILayerDebugger<T>
	{
		protected BoardLayer<T> layer;

		public ILayerDebugger(BoardLayer<T> layer)
		{
			this.layer = layer;
		}

		public abstract int[,] GetDebugState();
	}
}