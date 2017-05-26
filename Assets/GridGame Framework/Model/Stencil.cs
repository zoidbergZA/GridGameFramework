using System;
using System.Collections;
using System.Collections.Generic;
using GridGame;

public enum StencilFilter
{
	Fixed,
	Free,
	Option,
	Ignored
}

public struct Stencil : IGrid
{
	public StencilFilter[,] filter;

	public Vec2 Size { get; private set; }
	public Vec2 Key { get; private set; } // set 1 'Fixed' filter as Key

	public Stencil(StencilFilter[,] filter)
	{
		this.filter = filter;
		Size = new Vec2(filter.GetLength(0), filter.GetLength(1));
		
		//set Key - todo: optimize
		Key = Vec2.invalid;
		for (int x = 0; x < Size.x; x++)
		{
			for (int y = 0; y < Size.y; y++)
			{
				if (filter[x,y] == StencilFilter.Fixed)
				{
					Key = new Vec2(x, y);
				}
			}	
		}
	}

	public bool FitsInGrid(IGrid grid, Vec2 anchor)
	{
		Vec2 gridSize = grid.GetDimensions();

		if (anchor.x < 0 || anchor.y < 0)
			return false;

		if (anchor.x + Size.x > gridSize.x)
			return false;
		if (anchor.y + Size.y > gridSize.y)
			return false;
		
		return true;
	}

    public Vec2 GetDimensions()
    {
        return Size;
    }

	public Stencil Rotate90()
	{
		StencilFilter[,] rotatedFilter = new StencilFilter[Size.y, Size.x];

		//rotate filter values by 90 degrees
		for (int x = 0; x < Size.x; x++)
		{
			for (int y = 0; y < Size.y; y++)
			{
				//rotate and translate back to ++quadrant
				var rotatedPos = new Vec2(-y, x);
				rotatedPos.x += Size.y-1;

				rotatedFilter[rotatedPos.x, rotatedPos.y] = filter[x, y];
			}
		}

		Stencil result = new Stencil(rotatedFilter);

		return result;
	}
}
