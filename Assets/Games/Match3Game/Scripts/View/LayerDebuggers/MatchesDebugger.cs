using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;
using System;

namespace Match3
{
	public class MatchesDebugger : LayerDebugger
	{
		private BoardLayer<int> matchesLayer;
		private Vec2 size;

		public MatchesDebugger(LayerDebugView layerView, BoardLayer<int> matchesLayer) : base(layerView)
		{
			this.matchesLayer = matchesLayer;
			size = matchesLayer.GetDimensions();
		}

		public override void Refresh()
		{
			for (int i = 0; i < size.x; i++)
			{
				for (int j = 0; j < size.y; j++)
				{
					var cell = matchesLayer.cells[i,j];

					if (cell > 0) // cell == groupId
					{
						SetCellColor(new Vec2(i, j), Color.blue);
					}
					else if (cell < 0) // cell == resolvedId
					{
						SetCellColor(new Vec2(i, j), Color.red);
					}
				}	
			}
		}
	}
}