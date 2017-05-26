using System;
using System.Collections;
using System.Collections.Generic;

public abstract class ControllerPhase
{
    public PhaseState State { get; protected set; }
    public bool Disabled { get; private set; }

    public abstract BoardAlert[] Tick();

    public virtual void Start()
    {
        State = PhaseState.Working;
    }

    public virtual void Reset()
    {
        State = PhaseState.Ready;
    }
}

public enum PhaseState
{
    Ready,
    Working,
    Done
}