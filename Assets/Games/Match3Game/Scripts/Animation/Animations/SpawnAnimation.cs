using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;

namespace Match3
{
	public class SpawnAnimation : AAnimation 
	{
		public Vec2 position;

		public SpawnAnimation(Vec2 position)
		{
			this.position = position;
		}
	}
}