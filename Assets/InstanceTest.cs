using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceTest : MonoBehaviour
{
    public Abc abc = new Abc();
    void Start()
    {

        Debug.LogFormat("a {0}", abc.a);
        abc.a += 1;
        Debug.LogFormat("abc.a {0}", abc.a);
        PlusA(abc);
        Debug.LogFormat("PlusA {0}", abc.a);
        abc.PlusA(abc);
        Debug.LogFormat("abc.PlusA {0}", abc.a);        
        abc.DebugA();

    }

    private void PlusA(Abc abc)
    {
        abc.a += 1;
    }
}

public class Abc
{
    public int a = 1;

    public void PlusA(Abc abc)
    {
        abc.a +=1;
    }

    public void DebugA()
    {
        a += 1;
        Debug.LogFormat("Abc.PlusA {0}", a);
    }
}
