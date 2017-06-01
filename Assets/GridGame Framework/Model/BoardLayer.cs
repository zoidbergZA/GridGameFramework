﻿using System;
using System.Collections;
using System.Collections.Generic;
using GridGame;

public class BoardLayer<TLayer> : IGrid
{
	public TLayer[,] cells;

	public Vec2 Size { get; private set; }

	public BoardLayer(Vec2 size)
	{
		Size = size;

		cells = new TLayer[size.x, size.y];
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
}