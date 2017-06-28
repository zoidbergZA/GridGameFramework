using System.Collections;
using System.Collections.Generic;
using System;

namespace GridGame
{
	public class LayerDebugger<T>
	{
		protected BoardLayer<T> layer;
		private Vec2 size;
		private Func<T, string> converter;

		public LayerDebugger(BoardLayer<T> layer, Func<T, string> converter)
		{
			this.layer = layer;
			this.converter = converter;

			size = layer.GetDimensions();
		}

        public string[,] GetLayerState()
		{
			string[,] output = new string[size.x, size.y];

			for (int x = 0; x < size.x; x++)
			{
				for (int y = 0; y < size.y; y++)
				{
					output[x, y] = converter(layer.cells[x, y]);
				}
			}

			return output;
		}
	}
}