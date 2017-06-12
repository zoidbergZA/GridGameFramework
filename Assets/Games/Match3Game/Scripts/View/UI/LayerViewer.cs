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
		private BoardController<SwapInput> controller;
		private Dictionary<IDebugable, LayerSelector> layerMap = new Dictionary<IDebugable, LayerSelector>();
		private CellDebugView[,] cellViews;
		private float opacity = 0.3f;

		public void Init(Board board, BoardController<SwapInput> controller)
		{
			this.board = board;
			this.controller = controller;

			CreateDebugPanel();
			CreateCellViews();
			CreateSelectorButtons();

			controller.DebugEvent += OnDebugEvent;			
		}

		public void RefreshView(IDebugable layer)
		{
			Debug.Log("refresh layer: " + layer.GetLayerName());

			var debugInfo = layer.GetLayerState();

			for (int x = 0; x < debugInfo.GetLength(0); x++)
			{
				for (int y = 0; y < debugInfo.GetLength(1); y++)
				{
					cellViews[x, y].Refresh(Color.white, debugInfo[x, y]);
				}
			}
		}

		private void CreateSelectorButtons()
		{
			foreach (var kvp in board.layers)
			{
				var selector = Instantiate(protoLayerSelector);
				selector.transform.SetParent(protoLayerSelector.transform.parent);
				selector.transform.localScale = Vector3.one;
				selector.Init(this, kvp.Value);
				layerMap.Add(kvp.Value, selector);
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
			cellView.cellBackground.color = new Color(1, 1, 1, opacity);

			return cellView;
		}

		private void OnDebugEvent(IDebugable layer)
		{
			RefreshView(layer);
		}
	}
}