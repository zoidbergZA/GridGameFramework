using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;
using System;

public abstract class AGame<TInput> : MonoBehaviour, IGenericGame
{
	public bool startInDebug;
	public LayerViewer layerViewer;
	public GameDebugView gameDebugView;

	public Game<TInput> Game { get; private set; }
	public bool DebugMode { get; protected set; }
	public bool TickStepped { get; set; }
	public GameStates GameState { get { return Game.GameState; } }

	public abstract void HandleManualTick();
	protected abstract void OnInputHandled();
	protected abstract void OnPhaseEnded(int phase, string phaseName);
	protected abstract void OnTurnEnded(bool cancelled);

	public int GetTurn()
    {
        return Game.BoardController.Turn;
    }

	public int GetTicks()
	{
		return Game.BoardController.Ticks;
	}

	public ControllerState GetControllerState()
	{
		return Game.BoardController.State;
	}

	public ControllerPhase GetCurrentPhase()
	{
		if (Game.BoardController.CurrentPhase < 0)
			return null;

		return Game.BoardController.Phases[Game.BoardController.CurrentPhase];
	}

	protected virtual void StartGame(Board board, BoardController<TInput> controller)
	{
		DebugMode = startInDebug;
		Game = new Game<TInput>(board, controller);
		Game.Start();

		gameDebugView.gameObject.SetActive(DebugMode);
		layerViewer.gameObject.SetActive(DebugMode);

		controller.TurnEnded += OnTurnEnded;
		controller.PhaseEnded += OnPhaseEnded;
		controller.InputHandled += OnInputHandled;
		controller.DebugEvent += OnDebugEvent;
	}

	protected virtual void EndGame()
	{
		Game.End();
		
		Game.BoardController.TurnEnded -= OnTurnEnded;
		Game.BoardController.PhaseEnded -= OnPhaseEnded;
		Game.BoardController.InputHandled -= OnInputHandled;
		Game.BoardController.DebugEvent -= OnDebugEvent;
	}

	private void OnDebugEvent(IGenericLayer layer)
	{
		if (!DebugMode)
			return;

		layerViewer.SelectLayer(layer, true);
	}
}
