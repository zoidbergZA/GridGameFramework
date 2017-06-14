using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;

namespace Match3
{
	public class Match3Game : AGame<SwapInput>
	{
		public readonly Vec2 BOARD_SIZE = new Vec2(7, 7);

		public TextAsset levelFile;
		public TextAsset replayFile;
		public bool tickStepped;
		public bool alertStepped;
		public BoardView boardView;
		public LayerViewer layerViewer;
		public GameDebugView gameDebugView;
				
		private LevelLoader levelLoader = new LevelLoader();
		private ReplayController replayController;
		private BoardAlert[] lastTickAlerts = new BoardAlert[0];
	
		//board and layers
		private int fieldsLayerId;
		private int matchesLayerId;
		private int candidatesLayerId;
		private int trickeLayerId;

		public Level Level { get; set; }
		public ScoreKeeper ScoreKeeper { get; private set; }
		public int MovesLeft { get { return Level.moves - Game.BoardController.Turn; } }
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
		}

		private void StartGame()
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
			var board = new Board(BOARD_SIZE);
			var debuggers = new LayerDebuggers();

			var fieldsLayer = new BoardLayer<Field>("Fields", BOARD_SIZE, debuggers.FieldsDebugger);
			var matchesLayer = new BoardLayer<int>("Matches", BOARD_SIZE, debuggers.MatchesDebugger);
			var candidatesLayer = new BoardLayer<int>("Candidates", BOARD_SIZE, debuggers.CandidatesDebugger);
			var trickleLayer = new BoardLayer<TrickleState>("Trickle", BOARD_SIZE, debuggers.TrickleDebugger);

			fieldsLayerId = board.AddLayer(fieldsLayer);
			matchesLayerId = board.AddLayer(matchesLayer);
			candidatesLayerId = board.AddLayer(candidatesLayer);
			trickeLayerId = board.AddLayer(trickleLayer);

			ScoreKeeper = new ScoreKeeper(Level);
			
			var controller = InitController(board, ScoreKeeper);

			levelLoader.LoadLevel(Level, fieldsLayer);

			//register event listeners
			ScoreKeeper.ScoreChanged += OnScoreChanged;

			//init views and HUD
			boardView.InitView(fieldsLayer);
			layerViewer.Init(board, controller, debuggers, true);
			gameDebugView.Init(this, controller);
			GameManager.Instance.hud.Init(this);

			StartGame(board, controller);
			StartedAt = Time.time;

			Debug.Log("match-3 game started! board state: " + Game.BoardController.State);
		}

		protected void EndGame(bool success)
		{
			EndGame();

			replayController.SaveReplay();

			GameManager.Instance.hud.gameOverView.Show(success);

			//unregister event listeners
			ScoreKeeper.ScoreChanged -= OnScoreChanged;
		}

		public void HandleInput(SwapInput swapInput)
		{
			if (!IsValidInput(swapInput))
				return;

			var inputResult = Game.BoardController.HandleInput(swapInput);
			Debug.Log("input handled! valid input? " + inputResult + ", board state: " + Game.BoardController.State);
		
			if (inputResult && !tickStepped)
			{
				StartCoroutine(HandleTickLoop());
			}
		}

		public void HandleManualTick()
		{
			if (tickStepped && Game.GameState == GameStates.Running)
			{
				if (Game.BoardController.State == ControllerState.Working && !boardView.animationController.Playing)
				{
					HandleTick();
					boardView.animationController.PlayAnimations();
					
					PrintTickResults(lastTickAlerts);
				}
			}
		}

		public bool IsValidInput(SwapInput input)
		{
			var fieldsLayer = Game.Board.GetLayer<Field>(fieldsLayerId);

			if (Game.BoardController.State != ControllerState.ReadyForInput || boardView.animationController.Playing)
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
			while (Game.BoardController.State == ControllerState.Working)
			{
				HandleTick();
				float animationTime = boardView.animationController.PlayAnimations();

				if (animationTime > 0)
				{
					yield return new WaitForSeconds(animationTime);
				}
			}
		}

		private void HandleTick()
		{
			lastTickAlerts = Game.BoardController.Tick();
		}

		private BoardController<SwapInput> InitController(Board board, ScoreKeeper scoreKeeper)
		{
			//create board controller
			var boardController = new BoardController<SwapInput>();

			//add processor phases
			var moveProcessor = new MoveProcessor(board, fieldsLayerId, boardController, boardView.animationController);
			var matchProcessor = new MatchProcessor(board, fieldsLayerId, matchesLayerId, candidatesLayerId);
			var badMoveProcessor = new BadMoveProcessor(moveProcessor, matchProcessor, boardController);
			var resolver = new MatchResolver(matchProcessor, boardView.animationController, scoreKeeper);
			var candidateProcessor = new CandidateProcessor(board, fieldsLayerId, candidatesLayerId, matchProcessor);
			var trickler = new Trickler(boardController, board, fieldsLayerId, trickeLayerId, 
				matchProcessor, resolver, boardView.animationController);
			var shuffler = new ShuffleProcessor(board, fieldsLayerId, candidateProcessor, 
				boardView.animationController, boardController);

			boardController.AddPhase(moveProcessor, board.GetLayer<Field>(fieldsLayerId));
			boardController.AddPhase(matchProcessor, board.GetLayer<int>(matchesLayerId));
			boardController.AddPhase(badMoveProcessor, null);
			boardController.AddPhase(resolver, board.GetLayer<int>(matchesLayerId));
			boardController.AddPhase(trickler, board.GetLayer<TrickleState>(trickeLayerId));
			boardController.AddPhase(candidateProcessor, board.GetLayer<int>(candidatesLayerId));
			boardController.AddPhase(shuffler, null);

			return boardController;
		}

		protected override void OnInputHandled()
		{
			Debug.Log("input handled event!");
		}

		protected override void OnTurnEnded(bool cancelled)
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

		protected override void OnPhaseEnded(int phase, string phaseName)
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
				Debug.Log(result.message);
				counter++;
			}
		}
	}	
}