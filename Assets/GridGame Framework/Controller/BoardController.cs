﻿using System.Collections;
using System.Collections.Generic;

namespace GridGame
{
    public enum ControllerState { Disabled, ReadyForInput, Working }

    public class BoardController<TInput>
    {
        public delegate void Evt();
        public delegate void TurnEvt(bool cancelled);
        public delegate void PhaseEvt(int phase, string phaseName);
        public delegate void DebugEvt(IGenericLayer layer);

        public event Evt InputHandled;
        public event TurnEvt TurnEnded;
        public event PhaseEvt PhaseEnded;
        public event DebugEvt DebugEvent;

        public List<ControllerPhase> Phases { get; private set;}

        private Dictionary<ControllerPhase, IGenericLayer> phaseLayerMap = new Dictionary<ControllerPhase, IGenericLayer>(); 

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
            Phases = new List<ControllerPhase>();
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

        public void AddPhase(ControllerPhase phase, IGenericLayer debugLayer = null)
        {
            Phases.Add(phase);
            phaseLayerMap.Add(phase, debugLayer);
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

            if (InputHandled != null)
            {
                InputHandled();
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
                return new BoardAlert[] { new BoardAlert(null, "turn cancelled") };

            if (phaseLayerMap[Phases[CurrentPhase]] != null)
            {
                if (DebugEvent != null)
                {
                    DebugEvent(phaseLayerMap[Phases[CurrentPhase]]);
                }
            }

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
}