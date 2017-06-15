using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;

namespace Twenty48
{
	public class Twenty48Game : AGame<MoveDirection> 
	{
		public readonly Vec2 BOARD_SIZE = new Vec2(4, 4);

		public BoardView boardView;

		//layers
		private int tilesLayerId;
		private int gravityLayerId;

		private BoardAlert[] lastTickAlerts = new BoardAlert[0];

		public override void HandleInput(MoveDirection moveDirection)
		{
			var inputResult = Game.BoardController.HandleInput(moveDirection);
		
			if (inputResult && !TickStepped)
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
			var board = new Board(BOARD_SIZE);
			var layerDebugger = new LayerDebugger();

			tilesLayerId = board.AddLayer(new BoardLayer<int>("Tiles", BOARD_SIZE, layerDebugger.TilesDebugger));
			gravityLayerId = board.AddLayer(new BoardLayer<GravityState>("Gravity", BOARD_SIZE, layerDebugger.GravityDebugger));

			var controller = InitController(board);

			boardView.Init(BOARD_SIZE);

			//init random starting tile
			var pos = new Vec2(Random.Range(0, BOARD_SIZE.x), Random.Range(0, BOARD_SIZE.y));
			board.GetLayer<int>(tilesLayerId).cells[pos.x, pos.y] = 1;
			boardView.CreateTileView(pos);

			StartGame(board, controller, layerDebugger);
		}

		private BoardController<MoveDirection> InitController(Board board)
		{
			//init board controller
			var boardController = new BoardController<MoveDirection>();
		
			//add controller phases
			var gravityLayer = board.GetLayer<GravityState>(gravityLayerId);
			var tileLayer = board.GetLayer<int>(tilesLayerId);

			boardController.AddPhase(new GravityProcessor(boardController, gravityLayer, tileLayer), gravityLayer);

			return boardController;
		}

		private IEnumerator HandleTickLoop()
		{
			while (Game.BoardController.State == ControllerState.Working)
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

		public override void HandleManualTick()
		{
			if (TickStepped)
			{
				if (Game.BoardController.State == ControllerState.Working)
				{
					HandleTick();
					// boardView.animationController.PlayAnimations();
				}
			}
		}

		private void HandleTick()
		{
			lastTickAlerts = Game.BoardController.Tick();
		}

        protected override void OnInputHandled()
        {
            // throw new NotImplementedException();
        }

        protected override void OnPhaseEnded(int phase, string phaseName)
        {
            // throw new NotImplementedException();
        }

        protected override void OnTurnEnded(bool cancelled)
        {
            // throw new NotImplementedException();
        }
    }
}