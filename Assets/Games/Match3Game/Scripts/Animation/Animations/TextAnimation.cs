using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;

namespace Match3
{
	public class TextAnimation : AAnimation 
	{
		public Vec2 cell;
		public string text;
		public float duration;

		public TextAnimation(Vec2 cell, string text, float duration)
		{
			this.cell = cell;
			this.text = text;
			this.duration = duration;
		}
	}
}