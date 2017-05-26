using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;
using System;

namespace Match3
{
	public class CandidatesDebugger : LayerDebugger
	{
		private Vec2 size;
		private BoardLayer<int> candidatesLayer;

		public CandidatesDebugger(LayerDebugView layerView, BoardLayer<int> candidatesLayer) : base(layerView)
		{
			this.candidatesLayer = candidatesLayer;
			size = candidatesLayer.GetDimensions();
		}

		public override void Refresh()
		{
			for (int i = 0; i < size.x; i++)
			{
				for (int j = 0; j < size.y; j++)
				{
					var cell = candidatesLayer.cells[i,j];

					if (cell == 1)
					{
						// SetCellColor(new Vec2(i, j), Color.gray);
					}
					else if (cell == 2)
					{
						// SetCellColor(new Vec2(i, j), Color.white);
					}
					else if (cell == 3)
					{
						SetCellColor(new Vec2(i, j), Color.white);
					}
				}	
			}
		}
	}
}