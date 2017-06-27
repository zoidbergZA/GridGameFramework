using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GridGame;

public class LayerViewer : MonoBehaviour 
{
	public RectTransform boardRect;
	public CellDebugView cellViewPrefab;
	public LayerSelector protoLayerSelector;
	public Toggle showTextToggle;
	public Slider opacitySlider;
	public Toggle enabledToggle;

	private RectTransform boardDebugRect;
	private Board board;
	private IDebugColorizer colorizer;
	private Dictionary<IGenericLayer, LayerSelector> layerMap = new Dictionary<IGenericLayer, LayerSelector>();
	private IGenericLayer selectedLayer;
	private CellDebugView[,] cellViews;
	private float opacity = 0.3f;
	private float opacityChangedAt = float.MinValue;

	public bool IsInitialized { get; private set; }
	public bool IsOn 
	{ 
		get { return enabledToggle.isOn; } 
		private set { enabledToggle.isOn = value; }
	}

	public void Init(Board board, IDebugColorizer colorizer = null, bool startOn = true)
	{
		this.board = board;
		this.colorizer = colorizer;

		opacity = opacitySlider.value;
		IsOn = startOn;

		CreateDebugPanel();
		CreateCellViews();
		CreateSelectorButtons();
		ToggleIsOn(IsOn);

		enabledToggle.onValueChanged.AddListener(OnToggleEnabled);

		IsInitialized = true;			
	}

	public void SelectLayer(IGenericLayer layer, bool forceSelect = false)
	{
		if (selectedLayer != null)
		{
			layerMap[selectedLayer].HandleViewSelected(false);
		}

		if (selectedLayer == layer && !forceSelect)
		{
			selectedLayer = null;
			ClearView();
		}
		else
		{
			selectedLayer = layer;
			layerMap[layer].HandleViewSelected(true);

			RefreshView();
		}
	}

	public void OnToggleEnabled(bool newValue)
	{
		if (!IsInitialized)
			return;

		ToggleIsOn(enabledToggle.isOn);
	}

	public void OnToggleShowText(bool newValue)
	{
		if (!IsInitialized)
			return;

		RefreshView();
	}

	public void OnOpacitySliderChange(float newValue)
	{
		// if (Time.time < opacityChangedAt + 0.1f)
		// 	return;

		opacity = opacitySlider.value;
		opacityChangedAt = Time.time;

		RefreshView();
	}

	private void ToggleIsOn(bool on)
	{

		boardDebugRect.gameObject.SetActive(on);
		
		if (on)
		{
			RefreshView();
		}
	}

	private void RefreshView()
	{
		if (selectedLayer == null)
		{
			ClearView();
			return;
		}

		if (selectedLayer.GetIsDebugable())
		{
			var debugInfo = selectedLayer.GetDebugState();

			for (int x = 0; x < debugInfo.GetLength(0); x++)
			{
				for (int y = 0; y < debugInfo.GetLength(1); y++)
				{
					var color = colorizer != null ? colorizer.GetColor(debugInfo[x, y]) : Color.clear;
					string debugText = showTextToggle.isOn ? debugInfo[x, y] : ""; 

					cellViews[x, y].Refresh(color, debugText, opacity);
				}
			}
		}
	}

	private void ClearView()
	{
		for (int x = 0; x < cellViews.GetLength(0); x++)
		{
			for (int y = 0; y < cellViews.GetLength(1); y++)
			{
				cellViews[x, y].Refresh(Color.clear, "", opacity);
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
}
