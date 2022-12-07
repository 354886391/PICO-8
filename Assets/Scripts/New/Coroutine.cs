using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coroutine
{
    public bool RemoveOnComplete = true;
    public bool UseFixedDeltaTime;
    private Stack<IEnumerator> enumerators;
    private float waitTimer;
    private bool ended;

    public bool Active { get; private set; }
    public bool Finished { get; private set; }

    public Coroutine(IEnumerator functionCall, bool removeOnComplete = true)
    {
        enumerators = new Stack<IEnumerator>();
        enumerators.Push(functionCall);
        RemoveOnComplete = removeOnComplete;
    }

    public Coroutine(bool removeOnComplete = true)
    {
        RemoveOnComplete = removeOnComplete;
        enumerators = new Stack<IEnumerator>();
    }

    public void Update()
    {
        ended = false;
        if (waitTimer > 0.0)
        {
            waitTimer -= UseFixedDeltaTime ? Time.fixedDeltaTime : Time.deltaTime;
        }
        else
        {
            if (enumerators.Count <= 0)
                return;
            IEnumerator enumerator = enumerators.Peek();
            if (enumerator.MoveNext() && !ended)
            {
                if (enumerator.Current is int)
                    waitTimer = (int)enumerator.Current;
                if (enumerator.Current is float)
                {
                    waitTimer = (float)enumerator.Current;
                }
                else
                {
                    if (!(enumerator.Current is IEnumerator))
                        return;
                    enumerators.Push(enumerator.Current as IEnumerator);
                }
            }
            else
            {
                if (ended)
                    return;
                enumerators.Pop();
                if (enumerators.Count != 0)
                    return;
                Active = false;
                Finished = true;
                if (!RemoveOnComplete)
                    return;
                // RemoveSelf();
            }
        }
    }

    public void Cancel()
    {
        Active = false;
        Finished = true;
        waitTimer = 0.0f;
        enumerators.Clear();
        ended = true;
    }

    public void Replace(IEnumerator functionCall)
    {
        Active = true;
        Finished = false;
        waitTimer = 0.0f;
        enumerators.Clear();
        enumerators.Push(functionCall);
        ended = true;
    }
}

