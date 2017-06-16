using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Twenty48
{
	public class HudView : MonoBehaviour 
	{
		public Text scoreText;

		public void UpdateScoreText(int score)
		{
			scoreText.text = "SCORE: " + score;
		}

		public void UpdateScoreText(string msg, bool red = false)
		{	
			if (red)
				scoreText.color = Color.red;

			scoreText.text = msg;
		}
	}
}
