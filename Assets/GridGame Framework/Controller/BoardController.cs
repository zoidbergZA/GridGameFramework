using System.Collections;
using System.Collections.Generic;
using GridGame;

public class BoardController<TInput>
{
    public delegate void Evt();
    public delegate void TurnEvt(bool cancelled);
    public delegate void PhaseEvt(int phase, string phaseName);

    public event Evt ValidInputHandled;
    public event TurnEvt TurnEnded;
    public event PhaseEvt PhaseEnded;

	public List<ControllerPhase> Phases = new List<ControllerPhase>();

    public ControllerState State { 
        get 
        {
            if (CurrentPhase == -2)
                return ControllerState.Disabled;
            else if (CurrentPhase == -1)
                return ControllerState.ReadyForInput;
            else if (CurrentPhase >= 0)
                return ControllerState.Working;
            else throw new System.Exception("invalid board controller phase: " + CurrentPhase);
        } 
    }
    public bool Interruptable { get; set; }
    public int Turn { get; private set; } 
    public int CurrentPhase { get; private set; } 
    public int Ticks { get; private set; }
    public TInput LastInput { get; private set; }
    public List<TInput> InputHistory { get; private set; }

    public BoardController()
    {
        CurrentPhase = -2; //set phase to Disabled
    }

    public void Start()
    {
        InputHistory = new List<TInput>();
        CurrentPhase = -1; //set phase to ReadyForInput
    }

    public void Stop()
    {
        CurrentPhase = -2;
    }

    public bool HandleInput(TInput input)
    {
        if (State != ControllerState.ReadyForInput && !Interruptable)
            return false;

        if (State == ControllerState.Working && Interruptable)
        {
            HandleTurnComplete(); //interupt if working && interuptable
        }

        LastInput = input;
        InputHistory.Add(input);

        if (ValidInputHandled != null)
        {
            ValidInputHandled();
        }

        StartNextPhase();
        return true;
    }

    public BoardAlert[] Tick()
    {
        if (State != ControllerState.Working)
            return null;

        //check if previous tick completed phase
        if (Phases[CurrentPhase].State == PhaseState.Done)
        {
            if (PhaseEnded != null)
            {
                PhaseEnded(CurrentPhase, Phases[CurrentPhase].GetType().ToString());
            }

            StartNextPhase();    
            return null;
        }

        var result = Phases[CurrentPhase].Tick();
    
        Ticks++;
        
        //check if tick cancelled the turn
        if (State == ControllerState.ReadyForInput)
            return new BoardAlert[] { new BoardAlert(Vec2.invalid, Vec2.invalid, "turn cancelled") };

        return result;
    }

    public void CancelTurn()
    {
        HandleTurnComplete(true);
    }

    private void StartNextPhase()
    {
        CurrentPhase++;
        Ticks = 0;

        while (true)
        {
            if (CurrentPhase >= Phases.Count)
            {
                HandleTurnComplete();
                break;
            }
            else
            {
                if (Phases[CurrentPhase].Disabled)
                {
                    CurrentPhase++;
                    continue;
                }
                else
                {
                    if (Phases[CurrentPhase].State != PhaseState.Ready)
                    {
                        throw new System.Exception("next phase is not in ready state, but was called to start! " + Phases[CurrentPhase].GetType());
                    }

                    Phases[CurrentPhase].Start();
                    break;       
                }
            }    
        }
    }

    public void JumpToPhase(ControllerPhase phase)
    {
        if (State != ControllerState.Working)
            throw new System.Exception("can't jump to phase if State is not working!");

        var index = GetPhaseIndex(phase);

        CurrentPhase = index;

        for (int i = index; i < Phases.Count; i++)
        {
            Phases[i].Reset();
        }

        Phases[CurrentPhase].Start();
    }

    private int GetPhaseIndex(ControllerPhase phase)
    {
        for (int i = 0; i < Phases.Count; i++)
        {
            if (Phases[i] == phase)
                return i;
        }

        throw new System.Exception("could not find phase! ");
    }

    private void HandleTurnComplete(bool cancelled = false)
    {
        if (!cancelled)
        {
            Turn++;
        }

        foreach (var phase in Phases)
        {
            phase.Reset();
        }

        //go to ready for input phase
        CurrentPhase = -1;

        if (TurnEnded != null)
        {
            TurnEnded(cancelled);
        }
    }
}

public enum ControllerState
{
    Disabled,
    ReadyForInput,
    Working
}
