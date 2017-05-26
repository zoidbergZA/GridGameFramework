using System.Collections;
using System.Collections.Generic;
using GridGame;
using UnityEngine;

namespace Match3
{
	[System.Serializable]
	public class Replay
	{
		public Random.State RandomState;
		public Level Level;
		public SwapInput[] InputHistory;
	}
}
