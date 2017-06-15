using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;

namespace Twenty48
{
    public class SpawnProcessor : ControllerPhase
    {
		private BoardView boardView;
		private GravityProcessor gravityProcessor;
		private BoardLayer<int> tileLayer;
		private Vec2 boardSize;

		public SpawnProcessor(BoardView boardView, GravityProcessor gravityProcessor, BoardLayer<int> tileLayer)
		{
			this.boardView = boardView;
			this.gravityProcessor = gravityProcessor;
			this.tileLayer = tileLayer;

			boardSize = tileLayer.GetDimensions();
		}

        public override BoardAlert[] Tick()
        {
			Debug.Log(" Gravity Moves: " + gravityProcessor.Moves);

			if (gravityProcessor.Moves > 0)
			{
				//find empty cell
				var emptyCells = new List<Vec2>();

				for (int x = 0; x < boardSize.x; x++)
				{
					for (int y = 0; y < boardSize.y; y++)
					{
						if (tileLayer.cells[x, y] == 0)
							emptyCells.Add(new Vec2(x, y));
					}
				}

				if (emptyCells.Count > 0)
				{
					//spawn tile in random empty cell
					var randPos = emptyCells[Random.Range(0, emptyCells.Count)];

					tileLayer.cells[randPos.x, randPos.y] = 1;
					boardView.CreateTileView(randPos);
				}
				
				//check game over
			}

			State = PhaseState.Done;
            return null;
        }
    }
}
