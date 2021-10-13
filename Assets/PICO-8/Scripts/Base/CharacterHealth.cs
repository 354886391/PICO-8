#define ENABLE_DEBUG
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    public const int TotalHealth = 100;

    private int _currentHealth;
    
    private System.Action _onDeathbed;

    public void Start()
    {
        
        foreach (var item in FindObjectsOfType<SpinDamage>())
        {
            item.HurtEnterEvent += DectionHurt;
        }
    }

    public void Initialize(System.Action callback)
    {
        _onDeathbed = callback;      
    }

    public void Restart()
    {
        _currentHealth = TotalHealth;        
        Console.LogError("Character death");
    }

    private void DectionHurt(int damage)
    {
        BeingHurt(damage);
        if (IsDeath())
        {
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
