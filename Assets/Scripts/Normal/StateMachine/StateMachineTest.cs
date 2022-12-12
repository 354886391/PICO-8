using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class StateMachineTest : MonoBehaviour
{
    public StateMachine machine;

    // Start is called before the first frame update
    void Start()
    {
        machine = new StateMachine(3);
        machine.SetCallbacks(0, update: IdleUpdate, begin: IdleBegin);
        machine.SetCallbacks(1, update: RunUpdate, begin: RunBegin, coroutine: RunCoroutine);
        machine.SetCallbacks(2, update: JumpUpdate, begin: JumpBegin);
        machine.State = 0;
    }

    private void Update()
    {
        machine.Update();
    }

    private void IdleBegin()
    {
        Debug.Log("IdleBegin");
    }

    private int IdleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("KeyCode.A");
            return 1;
        }
        else
        {
            return 0;
        }
    }
    private void RunBegin()
    {
        Debug.Log("RunBegin");
    }

    private int RunUpdate()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("KeyCode.A");
            return 2;
        }
        else
        {
            return 1;
        }
    }

    private IEnumerator RunCoroutine()
    {
        Debug.LogWarning("0 RunCoroutine");
        yield return 1f;
        Debug.LogWarning("1 RunCoroutine");
        yield return 2f;
        Debug.LogWarning("2 RunCoroutine");
    }

    private void JumpBegin()
    {
        Debug.Log("JumpBegin");
    }

    private int JumpUpdate()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("KeyCode.A");
            return 0;
        }
        else
        {
            return 2;
        }
    }
}
