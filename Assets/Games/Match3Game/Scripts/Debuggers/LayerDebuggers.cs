using System;
using System.Collections;
using System.Collections.Generic;
using GridGame;
using UnityEngine;

namespace Match3
{
	public class LayerDebuggers : IDebugColorizer
	{
		public Func<Field, string> FieldsDebugger = (field) => 
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

		public Func<int, string> MatchesDebugger = (cell) => 
		{
			if (cell == 0)
				return "none";
			else if (cell == 1)
				return "match";
			else if (cell == -1)
				return "matchResolved";
			return "null";
		};

		public Func<int, string> CandidatesDebugger = (cell) => 
		{
			if (cell == 0)
				return "none";
			else if (cell == 1)
				return "fixed";
			else if (cell == 2)
				return "free";
			else if (cell == 3)
				return "option";
			return "null";
		};

		public Func<TrickleState, string> TrickleDebugger = (cell) => 
		{
			return cell.ToString();
		};

        public Color GetColor(string debugState)
        {
            Color output = Color.clear;

			switch (debugState)
			{
				case "Pink":
					output = new Color(1f, 0.38f, 0.41f);
					break;
				case "Blue":
					output = new Color(0f, 0f, 1f);
					break;
				case "Orange":
					output = new Color(1f, 0.65f, 0f);
					break;
				case "Yellow":
					output = new Color(1f, 1f, 0f);
					break;
				case "Lila":
					output = new Color(0.8f, 0.65f, 0.8f);
					break;
				case "match":
					output = Color.blue;
					break;
				case "matchResolved":
					output = Color.red;
					break;
				case "option":
					output = Color.white;
					break;
				case "free":
					output = Color.green;
					break;
				case "Fixed":
					output = Color.white;
					break;
				case "Open":
					output = Color.red;
					break;
				case "Falling":
					output = Color.blue;
					break;
				case "Landed":
					output = Color.green;
					break;
			}

			return output;
        }
    }
}