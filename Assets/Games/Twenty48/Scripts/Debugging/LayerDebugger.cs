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

        public Color GetColor(string debugState)
        {
            return Color.clear;
        }
    }
}
