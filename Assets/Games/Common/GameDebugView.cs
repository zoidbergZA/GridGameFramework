using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GridGame;

public class GameDebugView : MonoBehaviour 
{
	public Text gameStateText;
	public Text turnText;
	public Text tickText;
	public Text phaseText;
	public Toggle manualTickToggle;
	public Button tickButton;

	private IGenericGame game;

	public bool Initialized { get; private set; }

	public void Init(IGenericGame game)
	{
		this.game = game;

		manualTickToggle.isOn = game.TickStepped;
		tickButton.interactable = manualTickToggle.isOn; 

		Initialized = true;
	}

	void Update()
	{
		if (!Initialized)
			return;

		gameStateText.text = "GameState: " + game.GameState.ToString();
		turnText.text = "Turns: " + game.GetTurn();
		tickText.text = "Ticks: " + game.GetTicks();

		var controllerState = game.GetControllerState();
		string phaseString = controllerState.ToString();

		if (controllerState == ControllerState.Working)
		{
			phaseString += ": " + game.GetCurrentPhase().GetType();
		}

		phaseText.text = phaseString;
	}

	public void OnToggleManualTick(bool on)
	{
		game.TickStepped = on;
		tickButton.interactable = manualTickToggle.isOn;
	}

	public void OnClickTick()
	{
		game.HandleManualTick();
	}
}
