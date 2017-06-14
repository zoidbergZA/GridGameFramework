using System;
using System.Collections;
using System.Collections.Generic;
using GridGame;

namespace Twenty48
{
    public enum GravityState { Open, Ready, Fixed }

    public class GravityProcessor : ControllerPhase
    {
        private BoardController<MoveDirection> controller;
        private BoardLayer<GravityState> gravityLayer;
        private int step;

        public GravityProcessor(BoardController<MoveDirection> controller, BoardLayer<GravityState> gravityLayer)
        {
            this.controller = controller;
            this.gravityLayer = gravityLayer;
        }

        public override BoardAlert[] Tick()
        {
            step++;

            if (step >= 3)
                State = PhaseState.Done;

            return null;
        }

        public override void Reset()
        {
            base.Reset();

            step = 0;
        }
    }
}
