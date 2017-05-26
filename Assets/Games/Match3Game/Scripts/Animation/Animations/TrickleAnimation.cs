using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;

namespace Match3
{
	public class TrickleAnimation : AAnimation 
	{
		public Vec2 from;
		public Vec2 to;

		public TrickleAnimation(Vec2 from, Vec2 to)
		{
			this.from = from;
			this.to = to;
		}
	}
}