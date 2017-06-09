using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;

namespace Twenty48
{
	public class Board
	{
		public BoardLayer<int> tilesLayer;
		public BoardLayer<int> gravityLayer;

		public Vec2 Size { get; private set; }

		public Board(Vec2 size)
		{
			Size = size;

			tilesLayer = new BoardLayer<int>(size);
			gravityLayer = new BoardLayer<int>(size);
		}
	}
}
