using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;

namespace Match3
{
	public enum GameState
	{
		Ready,
		Running,
		Ended
	}

	public class Match3Game : MonoBehaviour
	{
		public const int WIDTH = 7;
		public const int HEIGHT = 7;

		public TextAsset levelFile;
		public TextAsset replayFile;
		public bool tickStepped;
		public bool alertStepped;
		public BoardView boardView;
		public BoardDebugView debugView;
		public GameDebugView gameDebugView;
		public BoardController<SwapInput> boardController;
		
		private LevelLoader levelLoader = new LevelLoader();
		private ReplayController replayController;
		private BoardAlert[] lastTickAlerts = new BoardAlert[0];
		private Board board;
		
		public Level Level { get; set; }
		public ScoreKeeper ScoreKeeper { get; private set; }
		public GameState GameState { get; private set; }
		public int MovesLeft { get { return Level.moves - boardController.Turn; } }
		public float StartedAt { get; set; }
		public bool ReplayMode { get { return replayController.Replay != null; } }
		public Random.State InitialRandomState { get; private set; }

		void Start()
		{
			StartGame();
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.T))
			{
				HandleManualTick();
			}

			if (Input.GetKeyDown(KeyCode.I) && ReplayMode)
			{
				replayController.HandleReplayInput();
			}

			if (Input.GetKeyDown(KeyCode.S))
			{
				if (lastTickAlerts.Length > 0)
				{
					if (alertStepped)
					{
						debugView.ShowNextAlert();
					}
					else
					{
						while (debugView.AlertIndex < lastTickAlerts.Length)
						{
							debugView.ShowNextAlert();
						}
					}
				}
			}
		}

		public void StartGame()
		{
			replayController = new ReplayController(this);

			if (replayFile != null)
			{
				replayController.LoadReplay(replayFile);
				Random.state = replayController.Replay.RandomState;
			}
			else
			{
				string json = levelFile.ToString();
				Level = JsonUtility.FromJson<Level>(json);
				InitialRandomState = Random.state;
			}

			board = new Board(new Vec2(WIDTH, HEIGHT));
			ScoreKeeper = new ScoreKeeper(Level);
			
			InitControllers(ScoreKeeper);
			levelLoader.LoadLevel(Level, board.fieldsLayer);

			//register boardController event listeners
			boardController.TurnEnded += OnTurnEnded;
			boardController.PhaseEnded += OnPhaseEnded;
			boardController.ValidInputHandled += OnInputHandled;
			ScoreKeeper.ScoreChanged += OnScoreChanged;

			//init views and HUD
			boardView.InitView(board.fieldsLayer);
			debugView.InitView(board.fieldsLayer, board.matchesLayer, board.candidatesLayer, board.trickleLayer);
			gameDebugView.Init(this, boardController);
			GameManager.Instance.hud.Init(this);

			boardController.Start();
			GameState = GameState.Running;
			StartedAt = Time.time;

			Debug.Log("match-3 game started! board state: " + boardController.State);
		}

		public void EndGame(bool success)
		{
			boardController.Stop();
			GameState = GameState.Ended;

			replayController.SaveReplay();

			GameManager.Instance.hud.gameOverView.Show(success);

			//unregister boardController event listeners
			boardController.TurnEnded -= OnTurnEnded;
			boardController.PhaseEnded -= OnPhaseEnded;
			boardController.ValidInputHandled -= OnInputHandled;
			ScoreKeeper.ScoreChanged -= OnScoreChanged;
		}

		public void HandleInput(SwapInput swapInput)
		{
			if (!IsValidInput(swapInput))
				return;

			var inputResult = boardController.HandleInput(swapInput);
			Debug.Log("input handled! valid input? " + inputResult + ", board state: " + boardController.State);
		
			if (inputResult && !tickStepped)
			{
				StartCoroutine(HandleTickLoop());
			}
		}

		public void HandleManualTick()
		{
			if (tickStepped && GameState == GameState.Running)
			{
				if (boardController.State == ControllerState.Working && !boardView.animationController.Playing)
				{
					HandleTick();
					boardView.animationController.PlayAnimations();
					debugView.RefreshBoardAlerts(lastTickAlerts);
					
					PrintTickResults(lastTickAlerts);
				}
			}
		}

		public bool IsValidInput(SwapInput input)
		{
			if (boardController.State != ControllerState.ReadyForInput || boardView.animationController.Playing)
				return false;
			if (!input.from.IsValidPosition(board.fieldsLayer) || !input.to.IsValidPosition(board.fieldsLayer))
				return false;
			if (board.fieldsLayer.cells[input.from.x, input.from.y].Gem == null)
				return false;
			if (board.fieldsLayer.cells[input.to.x, input.to.y].Gem == null)
				return false;
			//check adjacent
			var delta = input.to - input.from;

			if (delta != Vec2.up && delta != Vec2.down && delta != Vec2.left && delta != Vec2.right)
				return false;

			return true;
		}

		private IEnumerator HandleTickLoop()
		{
			while (boardController.State == ControllerState.Working)
			{
				HandleTick();
				float animationTime = boardView.animationController.PlayAnimations();

				if (animationTime > 0)
				{
					yield return new WaitForSeconds(animationTime);
				}
				
				debugView.RefreshBoardAlerts(lastTickAlerts);
			}
		}

		private void HandleTick()
		{
			lastTickAlerts = boardController.Tick();
		}

		private void InitControllers(ScoreKeeper scoreKeeper)
		{
			//create board controller
			boardController = new BoardController<SwapInput>();

			//add processor phases
			var moveProcessor = new MoveProcessor(board, boardController, boardView.animationController);
			var matchProcessor = new MatchProcessor(board);
			var resolver = new MatchResolver(matchProcessor, boardView.animationController, scoreKeeper);
			var candidateProcessor = new CandidateProcessor(board, matchProcessor);

			boardController.phases.Add(moveProcessor);
			boardController.phases.Add(matchProcessor);
			boardController.phases.Add(new BadMoveProcessor(moveProcessor, matchProcessor, boardController));
			boardController.phases.Add(resolver);
			boardController.phases.Add(new Trickler(boardController, board, matchProcessor, resolver, boardView.animationController));
			boardController.phases.Add(candidateProcessor);
			boardController.phases.Add(new ShuffleProcessor(board, candidateProcessor, boardView.animationController, boardController));
		}

		private void OnInputHandled()
		{
			Debug.Log("input handled event!");
		}

		private void OnTurnEnded(bool cancelled)
		{
			Debug.Log("====>> Turn ended. cancelled? " + cancelled);

			//check game over conditions
			if (ScoreKeeper.Score >= ScoreKeeper.TargetScore)
			{
				Debug.Log("GAME OVER: SUCCESS!");
				EndGame(true);
			}
			else if (MovesLeft <= 0)
			{
				Debug.Log("GAME OVER: OUT OF MOVES!");
				EndGame(false);
			}
		}

		private void OnPhaseEnded(int phase, string phaseName)
		{
			Debug.Log("Phase [" + phase +  "] " + phaseName + " ended");
		}

		private void OnScoreChanged(int score)
		{
			if (score >= Level.targetScore)
				EndGame(true);
		}

		private void PrintTickResults(BoardAlert[] results)
		{
			if (results == null)
				return;

			Debug.Log("==== last tick results(" + results.Length + "): ====");

			int counter = 0;
			foreach (var result in results)
			{
				Debug.Log("tick process [" + counter + "]" + " from: " + result.from + ", to: " + result.to + ", context: " + result.context);
				counter++;
			}
		}
	}	
}