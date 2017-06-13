using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Match3
{
	public class Hud : MonoBehaviour 
	{
		public Text movesText;
		public Text scoreText;
		public RectTransform scoreBar;
		public GameOverView gameOverView;

		private Match3Game game;
		private float scoreBarMax;

		public bool Initialized { get; private set; }

		public void Init(Match3Game game)
		{
			this.game = game;

			scoreText.text = "0";
			scoreBarMax = scoreBar.sizeDelta.y;
			scoreBar.sizeDelta = new Vector2(scoreBar.sizeDelta.x, 0);

			Initialized = true;
		}

		void Update()
		{
			if (!Initialized)
				return;

			if (game.GameState == GameStates.Running)
			{
				scoreText.text = game.ScoreKeeper.Score.ToString();			
				UpdateScoreBar();
				UpdateMovesText();
			}
		}

		private void UpdateScoreBar()
		{
			float frac = game.ScoreKeeper.Score / (float)game.ScoreKeeper.TargetScore * scoreBarMax;
			scoreBar.sizeDelta = new Vector2(scoreBar.sizeDelta.x, frac);
		}

		private void UpdateMovesText()
		{
			movesText.text = game.MovesLeft.ToString();
		}
	}
}