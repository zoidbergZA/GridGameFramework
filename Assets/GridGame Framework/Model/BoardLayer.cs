using System;
using System.Collections;
using System.Collections.Generic;
using GridGame;

public class BoardLayer<TLayer> : IGrid, IGenericLayer
{
	public TLayer[,] cells;

	public string Name {get ; private set; }
	public Vec2 Size { get; private set; }
	public LayerDebugger<TLayer> LayerDebugger { get; private set; }
	public bool IsDebugable { get { return LayerDebugger != null; } }

	public BoardLayer(string name, Vec2 size, Func<TLayer, string> debuggerFunc = null)
	{
		Name = name;
		Size = size;

		cells = new TLayer[size.x, size.y];

		if (debuggerFunc != null)
		{
			LayerDebugger = new LayerDebugger<TLayer>(this, debuggerFunc);
		}
	}

    public Vec2 GetDimensions()
    {
        return Size;
    }

	public TLayer[,] GetSample(Stencil stencil, Vec2 anchor)
	{
		var samples = new TLayer[stencil.Size.x, stencil.Size.y];

		for (int x = 0; x < stencil.Size.x; x++)
		{
			for (int y = 0; y < stencil.Size.y; y++)
			{
				Vec2 pos = new Vec2(x + anchor.x, y + anchor.y);

				if (pos.IsValidPosition(this))
					samples[x, y] = cells[pos.x, pos.y];
				else 
					samples[x, y] = default(TLayer);
			}
		}

		return samples;
	}

    public string GetLayerName()
    {
        return Name;
    }

    public string[,] GetDebugState()
    {
        if (IsDebugable)
		{
			return LayerDebugger.GetLayerState();
		}
		else
		{
			return null;
		}
    }
}
