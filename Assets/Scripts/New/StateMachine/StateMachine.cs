using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class StateMachine
{
    private int curState;
    private int prevState;

    private Action[] ends;
    private Action[] begins;
    private Func<int>[] updates;
    private Func<IEnumerator>[] coroutines;
    private StateCoroutine curCoroutine;

    public bool Locked;
    public bool ChangedStates;

    public int State
    {
        get
        {
            return curState;
        }
        set
        {
            if (Locked || curState == value)
            {
                return;
            }
            ChangedStates = true;
            prevState = curState;
            curState = value;
            if (prevState != -1 && ends[prevState] != null)
            {
                ends[prevState]();
            }
            if (begins[curState] != null)
            {
                begins[curState]();
            }
            if (coroutines[curState] != null)
            {
                curCoroutine.Replace(coroutines[curState]());
            }
            else
            {
                curCoroutine.Cancel();
            }
        }
    }

    public StateMachine(int maxStates)
    {
        prevState = curState = -1;
        ends = new Action[maxStates];
        begins = new Action[maxStates];
        updates = new Func<int>[maxStates];
        coroutines = new Func<IEnumerator>[maxStates];
        curCoroutine = new StateCoroutine(false);
    }

    public void SetCallbacks(int state, Func<int> update, Func<IEnumerator> coroutine = null, Action begin = null, Action end = null)
    {
        updates[state] = update;
        begins[state] = begin;
        ends[state] = end;
        coroutines[state] = coroutine;
    }

    public void Update()
    {
        ChangedStates = false;
        if (updates[curState] != null)
        {
            State = updates[curState]();
        }
        if (curCoroutine.Active)
        {
            curCoroutine.Update();
        }

    }

    public static implicit operator int(StateMachine state)
    {
        return state.curState;
    }
}
