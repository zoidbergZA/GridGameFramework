using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;
using System;

namespace Match3
{
	public class CandidateProcessor : ControllerPhase
	{
		public List<MatchGroup> candidateGroups = new List<MatchGroup>();
		public BoardLayer<Field> fieldLayer;
		public BoardLayer<int> candidatesLayer;
		
		private Vec2 boardSize;
		private MatchProcessor matcher;
		private int candidateIndex;

		public int CandidateCount { get { return candidateGroups.Count; } }

		public CandidateProcessor(Board board, MatchProcessor matcher)
		{
			fieldLayer = board.fieldsLayer;
			candidatesLayer = board.candidatesLayer;
			this.matcher = matcher;

			boardSize = fieldLayer.GetDimensions();
		}

		public override void Start()
		{
			base.Start();

			candidateIndex = 0;
			candidateGroups.Clear();

			for (int i = 0; i < boardSize.x; i++)
			{
				for (int j = 0; j < boardSize.y; j++)
				{
					candidatesLayer.cells[i,j] = 0;
				}
			}
		}

		public override void Reset()
		{
			base.Reset();
		}

		public override BoardAlert[] Tick()
		{
			if (candidateIndex >= matcher.Matchers.Length)
			{
				State = PhaseState.Done;
				return null;
			}

			var results = matcher.Matchers[candidateIndex].FindCandidates();
			candidateIndex++;

			if (results.Count > 0)
			{
				candidateGroups.AddRange(results);
			}

			return new BoardAlert[] { new BoardAlert(Vec2.invalid, Vec2.invalid, "end of candidate matcher tick: " + results.Count + " candidates") };
		}
	}
}