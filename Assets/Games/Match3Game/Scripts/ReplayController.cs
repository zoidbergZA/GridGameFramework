using System.Collections;
using System.Collections.Generic;
using GridGame;
using UnityEngine;
using System.IO;

namespace Match3
{
	public class ReplayController
	{
		public Replay Replay { get; private set; }

		private Match3Game m3;
		private int replayInputIndex;

		public ReplayController(Match3Game game)
		{
			this.m3 = game;
		}

		public void LoadReplay(TextAsset replayFile)
		{
			Replay = JsonUtility.FromJson<Replay>(replayFile.ToString());

			m3.Level = Replay.Level;
			replayInputIndex = 0;
		}

		public void HandleReplayInput()
		{
			if (replayInputIndex >= Replay.InputHistory.Length)
				return;

			var nextInput = Replay.InputHistory[replayInputIndex];

			if (!m3.IsValidInput(nextInput))
				return;
		
			m3.HandleInput(nextInput);
			replayInputIndex++;
		}

		public void SaveReplay()
		{
			var replay = new Replay();
			replay.RandomState = m3.InitialRandomState;
			replay.InputHistory = m3.Game.BoardController.InputHistory.ToArray();
			replay.Level = m3.Level;
			var json = JsonUtility.ToJson(replay, true);
			SaveJsonFile("TestReplay.json", json);
		}

		private void SaveJsonFile(string filename, string json)
		{
			StreamWriter sw = new StreamWriter(filename);
			sw.Write(json);
			sw.Close();

			Debug.Log("replay saved: " + filename);
		}
	}	
}