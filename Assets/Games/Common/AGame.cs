using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;

public enum GameStates { Ready, Running, Ended }

public class AGame : MonoBehaviour 
{
	protected Board board;

	public bool DebugMode { get; protected set; }
	public GameStates GameState { get; protected set; }
}
