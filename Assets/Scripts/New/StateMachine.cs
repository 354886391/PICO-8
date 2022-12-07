using System;
using System.Collections;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private Action[] ends;
    private Action[] begins;
    private Func<int>[] updates;
    private Func<IEnumerator>[] coroutines;
    private Coroutine currentCoroutine;

    private int currentState;
    private int previousState;

    public bool locked;
    public bool changedStates;

    public int State
    {
        get
        {
            return currentState;
        }
        set
        {
            if (locked || currentState == value)
            {
                return;
            }
            changedStates = true;
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
        currentCoroutine = new Coroutine();
        currentCoroutine.RemoveOnComplete = false;
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
        changedStates = false;
        if (this.updates[currentState] != null)
            this.State = this.updates[currentState]();
        //if (!this.currentCoroutine.Active)
        //    return;
        this.currentCoroutine.Update();
        if (this.changedStates || !this.currentCoroutine.Finished)
            return;
    }

    public static implicit operator int(StateMachine state)
    {
        return state.currentState;
    }
}
