using System;
using System.Collections;
using System.Collections.Generic;
using GridGame;

namespace Match3
{
    public class MoveProcessor : ControllerPhase
    {
        public BoardLayer<Field> fieldLayer;
        public BoardController<SwapInput> controller;
        public AnimationController animationController;

        public MoveProcessor(Board board, int fieldsLayerId, BoardController<SwapInput> controller, AnimationController animationController)
        {
            fieldLayer = board.GetLayer<Field>(fieldsLayerId);
            this.controller = controller;
            this.animationController = animationController;
        }

        public override void Reset()
        {
            base.Reset();
        }

        public override BoardAlert[] Tick()
        {
            if (State != PhaseState.Working)
                throw new System.Exception("Tick called on Phase that is not in working state!");

            var fromCell = controller.LastInput.from;
            var toCell = controller.LastInput.to;

            //handle gems swap
            var toField = fieldLayer.cells[toCell.x, toCell.y];
            var fromField = fieldLayer.cells[fromCell.x, fromCell.y];
            var notifications = new List<BoardAlert>();

            if (fromField.Gem != null && toField.Gem != null)
            {
                var tempGem = toField.Gem;
                toField.SetGem(fromField.Gem);
                fromField.SetGem(tempGem);
            
                animationController.QueueAnimation(new SwapAnimation(fromCell, toCell));
            }

            State = PhaseState.Done;

            return notifications.ToArray();
        }
    }
}