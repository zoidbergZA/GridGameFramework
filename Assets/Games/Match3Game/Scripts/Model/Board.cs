using System.Collections;
using System.Collections.Generic;
using GridGame;

namespace Match3
{
	public class Board
	{
		public BoardLayer<Field> fieldsLayer;
		public BoardLayer<int> matchesLayer;
		public BoardLayer<int> candidatesLayer;
		public BoardLayer<TrickleState> trickleLayer;

		public Vec2 Size { get; private set; }

		public Board(Vec2 size)
		{
			Size = size;

			fieldsLayer = new BoardLayer<Field>(size);
			matchesLayer = new BoardLayer<int>(size);
			candidatesLayer = new BoardLayer<int>(size);
			trickleLayer = new BoardLayer<TrickleState>(size);
		}
	}
}