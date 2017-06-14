using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;



public abstract class AGame<TInput> : MonoBehaviour 
{
	public Game<TInput> Game { get; private set; }
	public bool DebugMode { get; protected set; }
	
	protected abstract void OnInputHandled();
	protected abstract void OnPhaseEnded(int phase, string phaseName);
	protected abstract void OnTurnEnded(bool cancelled);

	protected virtual void StartGame(Board board, BoardController<TInput> controller)
	{
		Game = new Game<TInput>(board, controller);
		Game.Start();

		controller.TurnEnded += OnTurnEnded;
		controller.PhaseEnded += OnPhaseEnded;
		controller.InputHandled += OnInputHandled;
	}

	protected virtual void EndGame()
	{
		Game.End();
		
	
		Game.BoardController.TurnEnded -= OnTurnEnded;
		Game.BoardController.PhaseEnded -= OnPhaseEnded;
		Game.BoardController.InputHandled -= OnInputHandled;
	}
}
