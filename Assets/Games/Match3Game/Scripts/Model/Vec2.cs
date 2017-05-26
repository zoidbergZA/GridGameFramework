using System;
using System.ComponentModel;
using System.Collections;
using UnityEngine;

namespace GridGame
{
	[System.Serializable]
	public partial struct Vec2
	{
		static public implicit operator Vec2(Vector2 vector2)
		{
			return new Vec2((int)vector2.x, (int)vector2.y);
		}

		static public implicit operator Vector2(Vec2 vec2)
		{
			return new Vector2(vec2.x, vec2.y);
		}
	}
}
