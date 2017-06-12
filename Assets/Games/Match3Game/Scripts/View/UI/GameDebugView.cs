using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GridGame;

namespace Match3
{
	public class GameDebugView : MonoBehaviour 
	{
		public Text gameStateText;
		public Text turnText;
		public Text tickText;
		public Text phaseText;
		public Toggle manualTickToggle;
		public Text scoreText;
		public Text goalText;
		public Text movesLeftText;

		private Match3Game game;
		private BoardController<SwapInput> controller;

		public void Init(Match3Game game, BoardController<SwapInput> controller)
		{
			this.game = game;
			this.controller = controller;

			manualTickToggle.isOn = game.tickStepped;
		}

		void Update()
		{
			if (game == null || controller == null)
				return;

			gameStateText.text = "GameState: " + game.GameState.ToString();
			turnText.text = "Turns: " + game.boardController.Turn;
			tickText.text = "Ticks: " + controller.Ticks;
			movesLeftText.text = "Moves Left: " + game.MovesLeft;

			string phaseString = controller.State.ToString();

			if (controller.State == ControllerState.Working)
			{
				phaseString += ": " + controller.Phases[controller.CurrentPhase].GetType();
			}

			phaseText.text = phaseString;
			scoreText.text = "score: " + game.ScoreKeeper.Score;
			goalText.text = "goal: " + game.ScoreKeeper.TargetScore;
		}

		public void OnToggleManualTick(bool on)
		{
			game.tickStepped = on;
		}

		public void OnClickTick()
		{
			game.HandleManualTick();
		}
	}
}