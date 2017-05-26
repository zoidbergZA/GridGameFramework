using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;
using System;

namespace Match3
{
	public class MatchResolver : ControllerPhase
	{
		private BoardLayer<Field> fieldLayer;
		private BoardLayer<int> matchesLayer;
		private MatchProcessor matchProcessor;
		private AnimationController animController;
		private ScoreKeeper scoreKeeper;

		public MatchResolver(MatchProcessor matchProcessor, AnimationController animController, ScoreKeeper scoreKeeper)
		{
			this.matchProcessor = matchProcessor;
			this.animController = animController;
			this.scoreKeeper = scoreKeeper;

			fieldLayer = matchProcessor.fieldLayer;
			matchesLayer = matchProcessor.matchesLayer;
			
		}

		public override BoardAlert[] Tick()
		{
			foreach (var matchGroup in matchProcessor.matchGroups)
			{
				switch (matchGroup.matchType)
				{
					case MatchType.LineMatch :
						ResolveLineMatch(matchGroup);
						break;
				}
			}

			State = PhaseState.Done;
			return null;
		}

		private void ResolveLineMatch(MatchGroup matchGroup)
		{
			foreach (var cell in matchGroup.cells)
			{
				matchesLayer.cells[cell.x, cell.y] = -1; //flag cell as resolved

				fieldLayer.cells[cell.x, cell.y].SetGem(null);
			}		

			var points = scoreKeeper.CalculatePointsScored(matchGroup);
			animController.QueueAnimation(new ExplodeAnimation(matchGroup.cells, points));
		}
	}
}