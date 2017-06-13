using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridGame 
{
	public interface IDebugColorizer
	{
		Color GetColor(string debugState);
	}
}
