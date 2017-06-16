using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;

namespace Twenty48
{
    public class SpawnProcessor : ControllerPhase
    {
		private Twenty48Game game;
		private BoardView boardView;
		private GravityProcessor gravityProcessor;
		private BoardLayer<int> tileLayer;
		private Vec2 boardSize;

		public SpawnProcessor(Twenty48Game game, BoardView boardView, GravityProcessor gravityProcessor, BoardLayer<int> tileLayer)
		{
			this.game = game;
			this.boardView = boardView;
			this.gravityProcessor = gravityProcessor;
			this.tileLayer = tileLayer;

			boardSize = tileLayer.GetDimensions();
		}

        public override BoardAlert[] Tick()
        {
			// Debug.Log(" Gravity Moves: " + gravityProcessor.Moves);

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
				Debug.Log(" Empty cells: " + emptyCells.Count);
				if (emptyCells.Count > 0)
				{
					//spawn tile in random empty cell
					var randPos = emptyCells[Random.Range(0, emptyCells.Count)];

					tileLayer.cells[randPos.x, randPos.y] = 1;
					boardView.CreateTileView(randPos);

					if (emptyCells.Count == 1)
					{
						CheckGameOver();
					}
				}
			}

			State = PhaseState.Done;
            return null;
        }

		private void CheckGameOver()
		{
			for (int x = 0; x < boardSize.x; x++)
			{
				for (int y = 0; y < boardSize.y; y++)
				{
					if (HasAdjacentRank(new Vec2(x, y)))
						return;
				}
			}

			//if no adjacent, game over
			game.StopGame();
		}

		private bool HasAdjacentRank(Vec2 cell)
		{
			int cellRank = tileLayer.GetCell(cell);

			if (CompareCells(cell, Vec2.left))
				return true;
			if (CompareCells(cell, Vec2.right))
				return true;
			if (CompareCells(cell, Vec2.up))
				return true;
			if (CompareCells(cell, Vec2.down))
				return true;

			return false;
		}

		private bool CompareCells(Vec2 selected, Vec2 direction)
		{
			var other = selected + direction;

			if (!other.IsValidPosition(tileLayer))
				return false;

			if (tileLayer.GetCell(selected) == tileLayer.GetCell(other))
				return true;
			else
				return false;
		}
    }
}
