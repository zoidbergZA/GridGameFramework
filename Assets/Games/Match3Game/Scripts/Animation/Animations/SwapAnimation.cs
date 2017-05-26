using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;

namespace Match3
{
	public class SwapAnimation : AAnimation
	{
		public Vec2 from;
		public Vec2 to;

		public SwapAnimation(Vec2 fromField, Vec2 toField)
		{
			from = fromField;
			to = toField;
		}
	}
}