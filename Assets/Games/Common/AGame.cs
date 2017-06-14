using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;

public enum GameStates { Ready, Running, Ended }

public abstract class AGame<TInput> : MonoBehaviour 
{
	public bool DebugMode { get; protected set; }
	public GameStates GameState { get; private set; }
	protected Board Board { get; private set; }
	public BoardController<TInput> BoardController { get; private set; }

	protected abstract void OnInputHandled();
	protected abstract void OnPhaseEnded(int phase, string phaseName);
	protected abstract void OnTurnEnded(bool cancelled);

	protected virtual void StartGame(Board board, BoardController<TInput> controller)
	{
		Board = board;
		BoardController = controller;

		BoardController.Start();
		GameState = GameStates.Running;

		controller.TurnEnded += OnTurnEnded;
		controller.PhaseEnded += OnPhaseEnded;
		controller.InputHandled += OnInputHandled;
	}

	protected virtual void EndGame()
	{
		BoardController.Stop();
		GameState = GameStates.Ended;
	
		BoardController.TurnEnded -= OnTurnEnded;
		BoardController.PhaseEnded -= OnPhaseEnded;
		BoardController.InputHandled -= OnInputHandled;
	}
}
