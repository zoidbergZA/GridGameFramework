﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;
using System;

namespace Match3
{
	public class BadMoveProcessor : ControllerPhase
	{
		BoardController<SwapInput> controller;
		MoveProcessor moveProcessor;
		MatchProcessor matchProcessor;
		BoardLayer<Field> fieldsLayer;

		public BadMoveProcessor(MoveProcessor moveProcessor, MatchProcessor matchProcessor, BoardController<SwapInput> controller)
		{
			this.moveProcessor = moveProcessor;
			this.matchProcessor = matchProcessor;
			this.controller = controller;
			fieldsLayer = moveProcessor.fieldLayer;
		}

		public override BoardAlert[] Tick()
		{
			var move = controller.LastInput;
			var matches = matchProcessor.matchGroups.Count;

			if (matches == 0)
			{			
				var fromField = fieldsLayer.cells[move.from.x, move.from.y];
				var toField = fieldsLayer.cells[move.to.x, move.to.y];

				var tempGem = toField.Gem;
				toField.SetGem(fromField.Gem);
				fromField.SetGem(tempGem);
			
				moveProcessor.animationController.QueueAnimation(new SwapAnimation(move.from, move.to));
				controller.CancelTurn();
				return null;
			}

			State = PhaseState.Done;
			return null;
		}
	}
}