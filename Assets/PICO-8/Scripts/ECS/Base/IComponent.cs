using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IComponent
{
    public virtual void AddComponent() { }

    public virtual Icomonent GetComponent<Icomonent>() { return default; }

    public virtual bool HasComponent<Icomonent>() { return false; }
}
