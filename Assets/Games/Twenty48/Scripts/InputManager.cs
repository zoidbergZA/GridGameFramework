using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Twenty48
{
	public class InputManager : MonoBehaviour 
	{
		public Twenty48Game game;

		void Update() 
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
				game.HandleInput(MoveDirection.Up);
			if (Input.GetKeyDown(KeyCode.DownArrow))
				game.HandleInput(MoveDirection.Down);
			if (Input.GetKeyDown(KeyCode.LeftArrow))
				game.HandleInput(MoveDirection.Left);
			if (Input.GetKeyDown(KeyCode.RightArrow))
				game.HandleInput(MoveDirection.Right);
		}
	}
}
