using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using GridGame;

namespace Match3
{
	public class Hud : MonoBehaviour 
	{
		public Text movesText;
		public Text scoreText;
		public RectTransform scoreBar;
		public GameOverView gameOverView;

		private Match3Game m3;
		private float scoreBarMax;

		public bool Initialized { get; private set; }

		public void Init(Match3Game game)
		{
			this.m3 = game;

			scoreText.text = "0";
			scoreBarMax = scoreBar.sizeDelta.y;
			scoreBar.sizeDelta = new Vector2(scoreBar.sizeDelta.x, 0);

			Initialized = true;
		}

		void Update()
		{
			if (!Initialized)
				return;

			if (m3.Game.GameState == GameStates.Running)
			{
				scoreText.text = m3.ScoreKeeper.Score.ToString();			
				UpdateScoreBar();
				UpdateMovesText();
			}
		}

		private void UpdateScoreBar()
		{
			float frac = m3.ScoreKeeper.Score / (float)m3.ScoreKeeper.TargetScore * scoreBarMax;
			scoreBar.sizeDelta = new Vector2(scoreBar.sizeDelta.x, frac);
		}

		private void UpdateMovesText()
		{
			movesText.text = m3.MovesLeft.ToString();
		}
	}
}