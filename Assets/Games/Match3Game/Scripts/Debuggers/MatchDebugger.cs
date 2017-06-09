using System;
using System.Collections;
using System.Collections.Generic;
using GridGame;

namespace Match3
{
    public class MatchDebugger : ILayerDebugger<int>
    {
        public MatchDebugger(BoardLayer<int> matchesLayer) : base(matchesLayer)
        {
        
        }

        public override int[,] GetDebugState()
        {
            Vec2 size = layer.GetDimensions();
            int[,] output = new int[size.x, size.y];

			for (int x = 0; x < layer.cells.GetLength(0); x++)
			{
				for (int y = 0; y < layer.cells.GetLength(1); y++)
				{
					output[x,y] = layer.cells[x,y];
				}	
			}

			return output;
        }
    }
}