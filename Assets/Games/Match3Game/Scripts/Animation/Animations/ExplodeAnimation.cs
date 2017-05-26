using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;

namespace Match3
{
	public class ExplodeAnimation : AAnimation 
	{
		public Vec2[] explodeCells;
		public int points;

		public ExplodeAnimation(Vec2[] cells, int points)
		{
			explodeCells = cells;
			this.points = points;
		}
	}
}