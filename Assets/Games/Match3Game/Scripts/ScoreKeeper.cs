using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;

namespace Match3
{
	public class ScoreKeeper
	{
		public delegate void ScoreEvt(int newScore);
		public event ScoreEvt ScoreChanged; 
		
		public int Score { get; private set; }
		public int TargetScore { get; private set; }
		public int BaseGemValue { get; private set; }

		public ScoreKeeper(Level level)
		{
			TargetScore = level.targetScore;
			BaseGemValue = level.gemValue;
		}

		public int CalculatePointsScored(MatchGroup matchGroup)
		{
			var matchColor = matchGroup.gemColor;
			int colorValue =  BaseGemValue;
			int score = colorValue * matchGroup.cells.Length;

			return score;
		}

		public void AddScore(int score)
		{
			Score += score;

			if (ScoreChanged != null)
			{
				ScoreChanged(Score);
			}
		}
	}
}