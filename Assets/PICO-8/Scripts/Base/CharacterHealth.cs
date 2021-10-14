#define ENABLE_DEBUG
using System;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    public const int TotalHealth = 100;

    private bool _isDeading;
    private int _currentHealth;
    private Action _onDeathbed;

    public bool IsDeading { get { return _isDeading; } }

    public void Start()
    {

        foreach (var item in FindObjectsOfType<SpinDamage>())
        {
            item.HurtEnterEvent += DectionHurt;
        }
    }

    public void Initialize(Action callback)
    {
        _onDeathbed = callback;
    }

    public void Restart()
    {
        _isDeading = false;
        _currentHealth = TotalHealth;
        Console.LogError("Character death");
    }

    private void DectionHurt(int damage)
    {
        BeingHurt(damage);
        if (IsDeath())
        {
            _isDeading = true;
            _onDeathbed?.Invoke();
        }
    }

    private void BeingHurt(int damage)
    {
        _currentHealth -= damage;
    }

    private bool IsDeath()
    {
        return _currentHealth <= 0;
    }

}
