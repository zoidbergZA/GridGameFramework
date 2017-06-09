using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;

namespace Match3
{
	public class ShuffleProcessor : ControllerPhase
	{
		private BoardLayer<Field> fieldsLayer;
		private CandidateProcessor candidateProcessor;
		private AnimationController animController;
		private BoardController<SwapInput> controller;
		private Board board;

		public ShuffleProcessor(Board board, int fieldsLayerId, CandidateProcessor candidateProcessor,	AnimationController animController, BoardController<SwapInput> controller)
		{
			this.board = board;
			this.fieldsLayer = board.GetLayer<Field>(fieldsLayerId);
			this.candidateProcessor = candidateProcessor;
			this.animController = animController;
			this.controller = controller;
		}

		public override void Reset()
		{
			base.Reset();
		}

		public override BoardAlert[] Tick()
		{
			if (candidateProcessor.CandidateCount > 0)
			{
				State = PhaseState.Done;
				return null;
			}
			else
			{
				HandleReshuffle();
				controller.JumpToPhase(candidateProcessor);
				return null;
			}
		}

		private void HandleReshuffle()
		{
			var newPositions = new List<Vec2>();
			var swapFields = new List<Field>();

			for (int x = 0; x < board.Size.x; x++)
			{
				for (int y = 0; y < board.Size.y; y++)
				{
					if (fieldsLayer.cells[x,y].HasGem)
					{
						newPositions.Add(new Vec2(x, y));
						swapFields.Add(fieldsLayer.cells[x,y]);
					}
				}
			}

			//handle shuffle gems
			foreach (var fromField in swapFields)
			{
				int rand = Random.Range(0, newPositions.Count);
				var newPos = newPositions[rand];
				newPositions.RemoveAt(rand);	
			
				var toField = fieldsLayer.cells[newPos.x, newPos.y];
				var tempGem = toField.Gem;
				toField.SetGem(fromField.Gem);
				fromField.SetGem(tempGem);
			
				animController.QueueAnimation(new SwapAnimation(fromField.position, newPos));
			}
		}
	}
}