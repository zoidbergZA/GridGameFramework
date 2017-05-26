using System.Collections;
using System.Collections.Generic;
using GridGame;

namespace Match3
{
	public class LevelLoader
	{
		public void LoadLevel(Level levelLayout, BoardLayer<Field> fieldLayer)
		{
			Vec2 size = fieldLayer.GetDimensions();

			//init empty fields
			for (int i = 0; i < size.y; i++)
			{
				for (int j = 0; j < size.x; j++)
				{
					var field = fieldLayer.cells[i,j] = new Field(new Vec2(i,j));
				}	
			}

			//init gems
			for (int i = 0; i < levelLayout.boardGems.Length; i++)
			{
				var gemColor = (GemColor)levelLayout.boardGems[i];
				
				if (gemColor != GemColor.None)
				{
					var gem = new Gem(gemColor);
					int col = i % size.x;
					int row = size.y -1 - ((i - col) / size.y);

					fieldLayer.cells[col, row].SetGem(gem);
				}
			}
		}
	}
}
