using System;
using UnityEngine;

public class SpinDamage : MonoBehaviour
{
    public const int Damage = 100;
    public event Action<int> HurtEnterEvent;
    public event Action<int> HurtStayEvent;
    public event Action<int> HurtExitEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HurtEnterEvent?.Invoke(Damage);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HurtStayEvent?.Invoke(Damage);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HurtExitEvent?.Invoke(Damage);
        }
    }
}
