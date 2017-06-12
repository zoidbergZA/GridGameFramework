using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GridGame;

namespace Match3
{
	public class LayerViewer : MonoBehaviour 
	{
		public RectTransform boardRect;
		public CellDebugView cellViewPrefab;
		public LayerSelector protoLayerSelector;

		private RectTransform boardDebugRect;
		private Board board;
		private LayerSelector[] layerSelectors;
		private CellDebugView[,] cellViews;

		public void Init(Board board)
		{
			this.board = board;

			CreateDebugPanel();
			CreateCellViews();
			CreateSelectorButtons();
			
		}

		private void CreateSelectorButtons()
		{
			layerSelectors = new LayerSelector[board.layers.Count];

			for (int i = 0; i < layerSelectors.Length; i++)
			{
				layerSelectors[i] = Instantiate(protoLayerSelector);
				layerSelectors[i].transform.SetParent(protoLayerSelector.transform.parent);
				layerSelectors[i].transform.localScale = Vector3.one;
				layerSelectors[i].Init("layer " + i);
			}

			protoLayerSelector.gameObject.SetActive(false);
		}

		private void CreateDebugPanel()
		{
			var go = new GameObject("Board Debug Panel", typeof(RectTransform));
			boardDebugRect = go.GetComponent<RectTransform>();
			boardDebugRect.SetParent(boardRect.transform.parent);
			boardDebugRect.anchorMin = boardRect.anchorMin;
			boardDebugRect.anchorMax = boardRect.anchorMax;
			boardDebugRect.pivot = boardRect.pivot;
			boardDebugRect.localScale = Vector3.one;
			boardDebugRect.anchoredPosition = boardRect.anchoredPosition;
			boardDebugRect.sizeDelta = boardRect.sizeDelta;
		}

		private void CreateCellViews()
		{
			cellViews = new CellDebugView[board.Size.x, board.Size.y];

			for (int x = 0; x < board.Size.x; x++)
			{
				for (int y = 0; y < board.Size.y; y++)
				{
					cellViews[x, y] = CreateCellView(new Vec2(x, y));
				}
			}
		}

		private CellDebugView CreateCellView(Vec2 position)
		{
			var cellSize = new Vector2(boardDebugRect.sizeDelta.x / board.Size.x, boardDebugRect.sizeDelta.y / board.Size.y);
			var cellView = Instantiate(cellViewPrefab) as CellDebugView;
			cellView.transform.SetParent(boardDebugRect.transform);
			cellView.RectTransform.localScale = Vector3.one;
			cellView.RectTransform.sizeDelta = cellSize;

			var viewPosition = new Vector2(position.x * cellSize.x, position.y * cellSize.y);
			cellView.RectTransform.anchoredPosition = viewPosition;

			return cellView;
		}
	}
}