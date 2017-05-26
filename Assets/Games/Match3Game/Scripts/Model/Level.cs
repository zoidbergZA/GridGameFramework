using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;

namespace Match3
{
	[System.Serializable]
	public class Level
	{
		public int moves;
		public int targetScore;
		public int gemValue;
		public Vec2 size;
		public int[] boardGems;
	}
}