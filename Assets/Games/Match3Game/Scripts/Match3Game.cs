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
		public static Dictionary<GemColor, Color> colorMap = new Dictionary<GemColor, Color>()
		{
			{ GemColor.Blue, Color.blue },
			{ GemColor.Orange, new Color(255, 178, 0) },
			{ GemColor.None, Color.white },
			{ GemColor.Pink, new Color(255,192,203) },
			{ GemColor.Yellow, Color.yellow },
			{ GemColor.Lila, Color.green },
		};

		public readonly Vec2 BOARD_SIZE = new Vec2(7, 7);

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
	
		//board and layers
		private Board board;
		private int fieldsLayerId;
		private int matchesLayerId;
		private int candidatesLayerId;
		private int trickeLayerId;

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

			//create board and layers
			board = new Board(BOARD_SIZE);
			var fieldsLayer = new BoardLayer<Field>("Fields", BOARD_SIZE);
			fieldsLayer.SetDebugger(new FieldsDebugger(fieldsLayer));
			var matchesLayer = new BoardLayer<int>("Matches", BOARD_SIZE);
			matchesLayer.SetDebugger(new MatchDebugger(matchesLayer));
			var candidatesLayer = new BoardLayer<int>("Candidates", BOARD_SIZE);
			var trickleLayer = new BoardLayer<TrickleState>("Trickle", BOARD_SIZE);

			fieldsLayerId = board.AddLayer(fieldsLayer);
			matchesLayerId = board.AddLayer(matchesLayer);
			candidatesLayerId = board.AddLayer(candidatesLayer);
			trickeLayerId = board.AddLayer(trickleLayer);

			ScoreKeeper = new ScoreKeeper(Level);
			
			InitControllers(ScoreKeeper);
			levelLoader.LoadLevel(Level, fieldsLayer);

			//register boardController event listeners
			boardController.TurnEnded += OnTurnEnded;
			boardController.PhaseEnded += OnPhaseEnded;
			boardController.ValidInputHandled += OnInputHandled;
			ScoreKeeper.ScoreChanged += OnScoreChanged;

			//init views and HUD
			boardView.InitView(fieldsLayer);
			debugView.InitView(fieldsLayer, matchesLayer, candidatesLayer, trickleLayer);
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
			var fieldsLayer = board.GetLayer<Field>(fieldsLayerId);

			if (boardController.State != ControllerState.ReadyForInput || boardView.animationController.Playing)
				return false;
			if (!input.from.IsValidPosition(fieldsLayer) || !input.to.IsValidPosition(fieldsLayer))
				return false;
			if (fieldsLayer.cells[input.from.x, input.from.y].Gem == null)
				return false;
			if (fieldsLayer.cells[input.to.x, input.to.y].Gem == null)
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
			var moveProcessor = new MoveProcessor(board, fieldsLayerId, boardController, boardView.animationController);
			var matchProcessor = new MatchProcessor(board, fieldsLayerId, matchesLayerId, candidatesLayerId);
			var resolver = new MatchResolver(matchProcessor, boardView.animationController, scoreKeeper);
			var candidateProcessor = new CandidateProcessor(board, fieldsLayerId, candidatesLayerId, matchProcessor);

			boardController.phases.Add(moveProcessor);
			boardController.phases.Add(matchProcessor);
			boardController.phases.Add(new BadMoveProcessor(moveProcessor, matchProcessor, boardController));
			boardController.phases.Add(resolver);
			boardController.phases.Add(new Trickler(boardController, board, fieldsLayerId, trickeLayerId, 
				matchProcessor, resolver, boardView.animationController));
			boardController.phases.Add(candidateProcessor);
			boardController.phases.Add(new ShuffleProcessor(board, fieldsLayerId, candidateProcessor, 
				boardView.animationController, boardController));
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