using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceTest : MonoBehaviour
{
    public Abc abc = new Abc();
    public Cde cde;
    void Start()
    {

        //Debug.LogFormat("a {0}", abc.a);
        //abc.a += 1;
        //Debug.LogFormat("abc.a {0}", abc.a);
        //PlusA(abc);
        //Debug.LogFormat("PlusA {0}", abc.a);
        //abc.PlusA(abc);
        //Debug.LogFormat("abc.PlusA {0}", abc.a);
        //abc.DebugA();

        //Cde cde = null;
        //cde.CreateCde(cde);
        //Debug.LogFormat("cde.Create {0}", abc.a);
        //cde.PlusC(cde);
        //Debug.LogFormat("cde.PlusC {0}", abc.a);

        abc.CreateCde(ref cde);
        cde.PlusC(cde);
        Debug.LogFormat("cde.PlusC {0}", cde.c);

    }

    private void PlusA(Abc abc)
    {
        abc.a += 1;
    }
}

public class Abc
{
    public int a = 1;

    public void CreateCde(ref Cde cde)
    {
        cde = new Cde();
    }

    public void PlusA(Abc abc)
    {
        abc.a += 1;
    }

    public void DebugA()
    {
        a += 1;
        Debug.LogFormat("Abc.PlusA {0}", a);
    }
}

public class Cde
{
    public int c = 1;

    public void CreateCde(Cde cde)
    {
        cde = new Cde();
    }

    public void PlusC(Cde cde)
    {
        cde.c += 1;
    }
}
