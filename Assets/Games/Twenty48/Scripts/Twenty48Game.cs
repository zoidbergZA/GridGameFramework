using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;

namespace Twenty48
{
	public class Twenty48Game : MonoBehaviour 
	{
		public int width = 4;
		public int height = 4;

		public bool tickStepped;
		public BoardView boardView;

		private Board board;
		private int tilesLayerId;
		private int gravityLayerId;

		private BoardController<MoveDirection> boardController;
		private BoardAlert[] lastTickAlerts = new BoardAlert[0];

		public void HandleInput(MoveDirection moveDirection)
		{
			var inputResult = boardController.HandleInput(moveDirection);
			Debug.Log("input handled! valid input? " + inputResult + ", board state: " + boardController.State);
		
			if (inputResult && !tickStepped)
			{
				StartCoroutine(HandleTickLoop());
			}
		}

		private void Start()
		{
			StartGame();
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.T))
			{
				HandleManualTick();
			}
		}

		private void StartGame()
		{
			//create board and layers
			Vec2 boardSize = new Vec2(width, height);
			board = new Board(boardSize);
			tilesLayerId = board.AddLayer(new BoardLayer<int>("Tiles", boardSize));
			gravityLayerId = board.AddLayer(new BoardLayer<int>("Gravity", boardSize));

			InitControllers();
			boardView.Init(boardSize);

			//init random starting tile
			var pos = new Vec2(Random.Range(0, boardSize.x), Random.Range(0, boardSize.y));
			board.GetLayer<int>(tilesLayerId).cells[pos.x, pos.y] = 1;
			boardView.CreateTileView(pos);

			//register boardController event listeners
			boardController.TurnEnded += OnTurnEnded;
			boardController.PhaseEnded += OnPhaseEnded;
			boardController.ValidInputHandled += OnInputHandled;

			boardController.Start();
		}

		private void EndGame()
		{
			//unregister boardController event listeners
			boardController.TurnEnded -= OnTurnEnded;
			boardController.PhaseEnded -= OnPhaseEnded;
			boardController.ValidInputHandled -= OnInputHandled;
		}

		private void InitControllers()
		{
			//init board controller
			boardController = new BoardController<MoveDirection>();
		
			//add controller phases
			boardController.phases.Add(new MoveProcessor());
			boardController.phases.Add(new GravityProcessor());
		}

		private IEnumerator HandleTickLoop()
		{
			while (boardController.State == ControllerState.Working)
			{
				HandleTick();

				yield return null; //temp

				// float animationTime = boardView.animationController.PlayAnimations();

				// if (animationTime > 0)
				// {
				// 	yield return new WaitForSeconds(animationTime);
				// }
				
				// debugView.RefreshBoardAlerts(lastTickAlerts);
			}
		}

		public void HandleManualTick()
		{
			if (tickStepped)
			{
				if (boardController.State == ControllerState.Working)
				{
					HandleTick();
					// boardView.animationController.PlayAnimations();
					// debugView.RefreshBoardAlerts(lastTickAlerts);
					
					// PrintTickResults(lastTickAlerts);
				}
			}
		}

		private void HandleTick()
		{
			lastTickAlerts = boardController.Tick();
		}

		private void OnInputHandled()
		{
			Debug.Log("input handled event!");
		}

		private void OnTurnEnded(bool cancelled)
		{
			Debug.Log("====>> Turn ended. cancelled? " + cancelled);
		}

		private void OnPhaseEnded(int phase, string phaseName)
		{
			Debug.Log("Phase [" + phase +  "] " + phaseName + " ended");
		}
	}
}