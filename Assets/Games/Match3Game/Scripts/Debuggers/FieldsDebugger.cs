using System;
using System.Collections;
using System.Collections.Generic;
using GridGame;

namespace Match3
{
    public class FieldsDebugger : ILayerDebugger<Field>
    {
		public FieldsDebugger(BoardLayer<Field> fieldsLayer) : base(fieldsLayer)
		{
			
		}

        public override int[,] GetDebugState()
        {
			Vec2 size = layer.GetDimensions();
            int[,] output = new int[size.x, size.y];

			for (int x = 0; x < layer.cells.GetLength(0); x++)
			{
				for (int y = 0; y < layer.cells.GetLength(1); y++)
				{
					var field = layer.cells[x,y];

					if (field != null)
					{
						if (field.Gem != null)
						{
							output[x,y] = (int)field.Gem.color;
						}
					}
				}	
			}

			return output;
        }
    }
}