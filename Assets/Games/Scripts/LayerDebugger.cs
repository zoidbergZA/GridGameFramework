using System.Collections;
using System.Collections.Generic;
using GridGame;
using UnityEngine;

public abstract class LayerDebugger
{
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