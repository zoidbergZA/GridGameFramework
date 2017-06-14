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

		private Match3Game m3;
		private BoardController<SwapInput> controller;

		public void Init(Match3Game game, BoardController<SwapInput> controller)
		{
			this.m3 = game;
			this.controller = controller;

			manualTickToggle.isOn = game.tickStepped;
		}

		void Update()
		{
			if (m3 == null || controller == null)
				return;

			gameStateText.text = "GameState: " + m3.Game.GameState.ToString();
			turnText.text = "Turns: " + m3.Game.BoardController.Turn;
			tickText.text = "Ticks: " + controller.Ticks;
			movesLeftText.text = "Moves Left: " + m3.MovesLeft;

			string phaseString = controller.State.ToString();

			if (controller.State == ControllerState.Working)
			{
				phaseString += ": " + controller.Phases[controller.CurrentPhase].GetType();
			}

			phaseText.text = phaseString;
			scoreText.text = "score: " + m3.ScoreKeeper.Score;
			goalText.text = "goal: " + m3.ScoreKeeper.TargetScore;
		}

		public void OnToggleManualTick(bool on)
		{
			m3.tickStepped = on;
		}

		public void OnClickTick()
		{
			m3.HandleManualTick();
		}
	}
}