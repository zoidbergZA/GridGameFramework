using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;
using System;

namespace Match3
{
	public class TrickleDebugger : LayerDebugger
	{
		private BoardLayer<TrickleState> trickleLayer;
		private Vec2 size;

		public TrickleDebugger(LayerDebugView layerView, BoardLayer<TrickleState> trickleLayer) : base(layerView)
		{
			this.trickleLayer = trickleLayer;
			size = trickleLayer.GetDimensions();
		}

		public override void Refresh()
		{
			for (int i = 0; i < size.x; i++)
			{
				for (int j = 0; j < size.y; j++)
				{
					var cell = trickleLayer.cells[i,j];

					switch (cell)
					{
						// case TrickleState.Default :
						// 	SetCellColor(new Vec2(i, j), Color.clear);
						// 	break;
						case TrickleState.Fixed :
							SetCellColor(new Vec2(i, j), Color.white);
							break;
						case TrickleState.Open :
							SetCellColor(new Vec2(i, j), Color.blue);
							break;
						case TrickleState.Falling :
							SetCellColor(new Vec2(i, j), Color.yellow);
							break;
						case TrickleState.Landed :
							SetCellColor(new Vec2(i, j), Color.green);
							break;
					}
				}	
			}
		}
	}
}