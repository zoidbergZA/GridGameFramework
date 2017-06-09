using System;
using System.Collections;
using System.Collections.Generic;
using GridGame;

namespace Match3
{
	public static class LayerDebuggers
	{
		public static Func<Field, string> FieldsDebugger = (field) => 
		{
			if (field != null)
			{
				if (field.Gem != null)
				{
					return field.Gem.color.ToString();
				}
			}

			return "null";
		};

		public static Func<int, string> MatchesDebugger = (cell) => 
		{
			if (cell == 0)
				return "none";
			else if (cell == 1)
				return "match";
			return "null";
		};

		public static Func<int, string> CandidatesDebugger = (cell) => 
		{
			if (cell == 0)
				return "none";
			else if (cell == 1)
				return "fixed";
			else if (cell == 2)
				return "free";
			else if (cell == 3)
				return "options";
			return "null";
		};

		public static Func<TrickleState, string> TrickleDebugger = (cell) => 
		{
			return cell.ToString();
		};
	}
}