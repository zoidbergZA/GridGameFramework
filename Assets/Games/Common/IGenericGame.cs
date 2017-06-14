using System.Collections;
using System.Collections.Generic;
using GridGame;

public interface IGenericGame
{
	bool DebugMode { get; }
	bool TickStepped { get; set; }
	GameStates GameState { get; }

	int GetTurn();
	int GetTicks();
	ControllerState GetControllerState();
	ControllerPhase GetCurrentPhase();
	void HandleManualTick();
}
