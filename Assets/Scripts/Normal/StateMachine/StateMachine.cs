using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class StateMachine
{
    private Action[] ends;
    private Action[] begins;
    private Func<int>[] updates;
    private Func<IEnumerator>[] coroutines;
    private StateCoroutine currentCoroutine;

    private int currentState;
    private int previousState;

    public bool Locked;
    public bool ChangedStates;

    public int State
    {
        get
        {
            return currentState;
        }
        set
        {
            if (Locked || currentState == value)
            {
                return;
            }
            ChangedStates = true;
            previousState = currentState;
            currentState = value;
            if (previousState != -1 && ends[previousState] != null)
            {
                ends[previousState]();
            }
            if (currentState != -1 && begins[currentState] != null)
            {
                begins[currentState]();
            }
            if (currentState != -1 && coroutines[currentState] != null)
            {
                currentCoroutine.Replace(coroutines[currentState]());
            }
            else
            {
                currentCoroutine.Cancel();
            }
        }
    }

    public StateMachine(int maxStates)
    {
        previousState = currentState = -1;
        ends = new Action[maxStates];
        begins = new Action[maxStates];
        updates = new Func<int>[maxStates];
        coroutines = new Func<IEnumerator>[maxStates];
        currentCoroutine = new StateCoroutine(false);
    }

    public void SetCallbacks(int state, Func<int> onUpdate, Func<IEnumerator> coroutine = null, Action begin = null, Action end = null)
    {
        updates[state] = onUpdate;
        begins[state] = begin;
        ends[state] = end;
        coroutines[state] = coroutine;
    }

    public void Update()
    {
        ChangedStates = false;
        if (currentState != -1 && updates[currentState] != null)
        {
            State = updates[currentState]();
        }
        if (currentCoroutine.Active)
        {
            currentCoroutine.Update();
        }
    }

    public static implicit operator int(StateMachine state)
    {
        return state.currentState;
    }
}
