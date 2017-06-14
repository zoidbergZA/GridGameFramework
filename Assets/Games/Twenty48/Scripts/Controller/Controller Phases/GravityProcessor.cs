using System;
using System.Collections;
using System.Collections.Generic;
using GridGame;

namespace Twenty48
{
    public class GravityProcessor : ControllerPhase
    {
        private BoardController<MoveDirection> controller;
        private int step;

        public GravityProcessor(BoardController<MoveDirection> controller)
        {
            this.controller = controller;
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
