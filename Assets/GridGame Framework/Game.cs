using System.Collections;
using System.Collections.Generic;

namespace GridGame
{
	public enum GameStates { Ready, Running, Ended }

	public class Game<TInput>
	{
		public Board Board { get; private set; }
		public BoardController<TInput> BoardController { get; private set; }
		public GameStates GameState { get; private set; }

		public Game(Board board, BoardController<TInput> controller)
		{
			Board = board;
			BoardController = controller;
		}

		public void Start()
		{
			BoardController.Start();
			GameState = GameStates.Running;
		}

		public void End()
		{
			BoardController.Stop();
			GameState = GameStates.Ended;
		}
	}
}
