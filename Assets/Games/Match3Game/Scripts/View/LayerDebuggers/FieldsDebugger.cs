using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;

namespace Match3
{
	public class FieldsDebbuggerOld : LayerDebugger
	{
		private BoardLayer<Field> fieldLayer;

		public FieldsDebbuggerOld(LayerDebugView layerView, BoardLayer<Field> fieldLayer) : base(layerView)
		{
			this.fieldLayer = fieldLayer;
		}

		public override void Refresh()
		{
			for (int i = 0; i < fieldLayer.cells.GetLength(0); i++)
			{
				for (int j = 0; j < fieldLayer.cells.GetLength(1); j++)
				{
					var field = fieldLayer.cells[i,j];

					if (field != null)
					{
						if (field.Gem != null)
						{
							SetCellColor(new Vec2(i, j), Match3Game.colorMap[field.Gem.color]);
						}
					}
				}	
			}
		}
	}
}