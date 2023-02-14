using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class StateCoroutine
{

    public bool RemoveOnComplete = true;
    public bool UseFixedDeltaTime;

    private bool ended;
    private float waitTimer;
    private Stack<IEnumerator> enumerators;


    /// <summary>
    /// 激活状态
    /// </summary>
    public bool Active { get; private set; }
    /// <summary>
    /// 已完成
    /// </summary>
    public bool Finished { get; private set; }

    public StateCoroutine(IEnumerator functionCall, bool removeOnComplete = true)
    {
        enumerators = new Stack<IEnumerator>();
        enumerators.Push(functionCall);
        RemoveOnComplete = removeOnComplete;
    }

    public StateCoroutine(bool removeOnComplete = true)
    {
        RemoveOnComplete = removeOnComplete;
        enumerators = new Stack<IEnumerator>();
    }

    /// <summary>
    /// 更新协程
    /// </summary>
    public void Update()
    {
        ended = false;
        if (waitTimer > 0f)
        {
            // 更新等待计时
            waitTimer -= UseFixedDeltaTime ? Time.fixedDeltaTime : Time.deltaTime;
        }
        else
        {
            // 等待超时
            if (enumerators.Count > 0)
            {
                // 获取下一个枚举器 e
                var enumerator = enumerators.Peek();
                if (enumerator.MoveNext() && !ended)
                {
                    // 更新 e 的等待计时器或添加新的枚举器
                    if (enumerator.Current is int)
                    {
                        waitTimer = (int)enumerator.Current;
                    }
                    else if (enumerator.Current is float)
                    {
                        waitTimer = (float)enumerator.Current;
                    }
                    else if (enumerator.Current is IEnumerator)
                    {
                        enumerators.Push(enumerator.Current as IEnumerator);
                    }
                }
                else
                {
                    if (!ended)
                    {
                        // 弹出当前枚举器
                        enumerators.Pop();
                        // 枚举器栈为空时 结束协程
                        if (enumerators.Count == 0)
                        {
                            Active = false;
                            Finished = true;
                            if (RemoveOnComplete)
                            {
                                RemoveSelf();
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 取消协程
    /// </summary>
    public void Cancel()
    {
        Active = false;
        Finished = true;

        ended = true;
        waitTimer = 0.0f;
        enumerators.Clear();
    }

    /// <summary>
    /// 清空栈并添加新迭代器
    /// </summary>
    /// <param name="functionCall"></param>
    public void Replace(IEnumerator functionCall)
    {
        Active = true;
        Finished = false;

        ended = true;
        waitTimer = 0.0f;
        enumerators.Clear();
        enumerators.Push(functionCall);
    }

    public void RemoveSelf()
    {
        Debug.LogWarning("RemoveSelf");
    }
}

