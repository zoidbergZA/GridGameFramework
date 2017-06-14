using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Match3
{
	public class M3GameView : MonoBehaviour 
	{
		public Text scoreText;
		public Text goalText;
		public Text movesLeftText;

		private bool initialized;
		private Match3Game match3;

		public void Init(Match3Game match3)
		{
			this.match3 = match3;
			initialized = true;
		}

		void Update()
		{
			if (!initialized)
				return;

			movesLeftText.text = "Moves Left: " + match3.MovesLeft;
			scoreText.text = "score: " + match3.ScoreKeeper.Score;
			goalText.text = "goal: " + match3.ScoreKeeper.TargetScore;			
		}
	}
}
