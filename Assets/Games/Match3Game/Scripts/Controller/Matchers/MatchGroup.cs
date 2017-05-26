using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GridGame;

namespace Match3
{
	public enum MatchType
	{
		LineMatch
	}

	public class MatchGroup : IGrouping<Vec2, Vec2>
	{
		public Vec2 key = Vec2.invalid;
		public Vec2[] cells;
		public MatchType matchType;
		public GemColor gemColor;

		public Vec2 Key { get { return key; } }

		public MatchGroup(Vec2 key, Vec2[] cells, MatchType matchType, GemColor gemColor)
		{
			this.key = key;
			this.cells = cells;
			this.matchType = matchType;
			this.gemColor = gemColor;
		}

		public IEnumerator<Vec2> GetEnumerator()
		{
			return cells.ToList().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return cells.GetEnumerator();
		}
	}
}