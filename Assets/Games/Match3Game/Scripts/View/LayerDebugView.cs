using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GridGame;
using System;

namespace Match3
{
	public class LayerDebugView : MonoBehaviour 
	{
		public Image panel;
		public Image protoCell;
		public float transparency = 0.2f;
		public Image[,] cells;

		private LayerDebugger layerDebugger;

		public void Init(string name, Vec2 dimensions, float cellSize, LayerDebugger layerDebugger)
		{
			this.layerDebugger = layerDebugger;
			cells = new Image[dimensions.x, dimensions.y];

			panel.rectTransform.sizeDelta = new Vector2(dimensions.x * cellSize, dimensions.y * cellSize);

			for (int i = 0; i < dimensions.x; i++)
			{
				for (int j = 0; j < dimensions.y; j++)
				{
					Image cell = Instantiate(protoCell);
					cell.transform.SetParent(protoCell.transform.parent);
					cell.transform.localScale = Vector3.one;
					cell.name = "[" + i + ", " + j + "]";
					cell.rectTransform.anchoredPosition = new Vector2(i, j) * cellSize;
					cell.color = Color.clear;

					cells[i,j] = cell;
				}	
			}

			protoCell.gameObject.SetActive(false);
		}

		public void Refresh()
		{
			layerDebugger.Refresh();
		}

		public void Clear()
		{
			layerDebugger.Clear();
		}
	}
}