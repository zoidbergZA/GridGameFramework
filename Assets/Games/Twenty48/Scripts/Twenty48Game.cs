﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;

namespace Twenty48
{
	public class Twenty48Game : AGame<MoveDirection> 
	{
		public readonly Vec2 BOARD_SIZE = new Vec2(4, 4);

		public float animSpeed = 1f;
		public BoardView boardView;
		public HudView hudView;

		private int tilesLayerId;
		private int gravityLayerId;
		private BoardAlert[] lastTickAlerts = new BoardAlert[0];
		private bool busy;

		public int Score { get; private set; }
		public bool GameOver { get; private set; }

		public override void HandleInput(MoveDirection moveDirection)
		{
			if (boardView.TileAnimator.IsPlaying)
				return;
			if (busy)
				return;

			var inputResult = Game.BoardController.HandleInput(moveDirection);
		
			if (inputResult && !TickStepped)
			{
				busy = true;
				StartCoroutine(HandleTickLoop());
			}
		}

		private void Start()
		{
			Cursor.visible = true;

			hudView.UpdateScoreText(Score);

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

			// //test layout
			// Vec2[] cells = new Vec2[]
			// {
			// 	new Vec2(0, 3),
			// 	new Vec2(1, 3),
			// 	new Vec2(2, 3),
			// 	new Vec2(3, 3)
			// };

			// foreach (var item in cells)
			// {
			// 	board.GetLayer<int>(tilesLayerId).cells[item.x, item.y] = 1;
			// 	boardView.CreateTileView(item);
			// }

			StartGame(board, controller, layerDebugger);
		}

		public void StopGame()
		{
			GameOver = true;
		}

		public void ScorePoints(int amount)
		{
			Score += amount;

			hudView.UpdateScoreText(Score);
		}

		private BoardController<MoveDirection> InitController(Board board)
		{
			//init board controller
			var boardController = new BoardController<MoveDirection>();
		
			//add controller phases
			var gravityLayer = board.GetLayer<GravityState>(gravityLayerId);
			var tileLayer = board.GetLayer<int>(tilesLayerId);

			var gravityProcessor = new GravityProcessor(this, boardController, boardView.TileAnimator, gravityLayer, tileLayer);

			boardController.AddPhase(gravityProcessor, gravityLayer);
			boardController.AddPhase(new SpawnProcessor(this, boardView, gravityProcessor, tileLayer), tileLayer);

			return boardController;
		}

		private IEnumerator HandleTickLoop()
		{
			while (Game.BoardController.State == ControllerState.Working)
			{
				HandleTick();
				yield return boardView.TileAnimator.PlayAnimations();
			}

			busy = false;
		}

		public override void HandleManualTick()
		{
			if (TickStepped && !boardView.TileAnimator.IsPlaying)
			{
				if (Game.BoardController.State == ControllerState.Working)
				{
					HandleTick();
					StartCoroutine(boardView.TileAnimator.PlayAnimations());
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
			if (GameOver)
			{
				hudView.UpdateScoreText("GAME OVER: " + Score, true);
				EndGame();
				Debug.Log("Game Over!");
			}
        }
    }
}