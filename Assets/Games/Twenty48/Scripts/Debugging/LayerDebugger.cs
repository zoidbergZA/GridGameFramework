using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;
using System;

namespace Twenty48
{
    public class LayerDebugger : IDebugColorizer
    {
		public Func<int, string> TilesDebugger = (cell) => 
		{
			return cell.ToString();
		};

		public Func<GravityState, string> GravityDebugger = (cell) => 
		{
			return cell.ToString();
		};

        public Color GetColor(string debugState)
        {
			var color = Color.clear;

			switch (debugState)
			{
				case "Open":
					color = Color.blue;
					break;
				case "Fixed":
					color = Color.white;
					break;
				case "Ready":
					color = Color.green;
					break;
			}

			return color;
        }
    }
}
