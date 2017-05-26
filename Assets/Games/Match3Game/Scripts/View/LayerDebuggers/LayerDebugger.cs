using System.Collections;
using System.Collections.Generic;
using GridGame;
using UnityEngine;

namespace Match3
{
	public abstract class LayerDebugger
	{
		protected static Dictionary<GemColor, Color> colorMap = new Dictionary<GemColor, Color>()
		{
			{ GemColor.Blue, Color.blue },
			{ GemColor.Orange, new Color(255, 178, 0) },
			{ GemColor.None, Color.white },
			{ GemColor.Pink, new Color(255,192,203) },
			{ GemColor.Yellow, Color.yellow },
			{ GemColor.Lila, Color.green },
		};

		protected LayerDebugView layerView;

		public LayerDebugger(LayerDebugView layerView)
		{
			this.layerView = layerView;
		}

		public virtual void Clear()
		{
			foreach (var cell in layerView.cells)
			{
				cell.color = Color.clear;
			}
		}

		public abstract void Refresh();

		protected void SetCellColor(Vec2 cell, Color color)
		{
			color.a = layerView.transparency;
			layerView.cells[cell.x, cell.y].color = color;
		}
	}
}