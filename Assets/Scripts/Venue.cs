using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Venue : MonoBehaviour
{
    public delegate void Destroyed();
    public event Destroyed OnDestroyed;

    private delegate void Damaged(float currentHP);
    private event Damaged OnDamaged;

    private float _health;

    private void Awake()
    {
        OnDamaged += OnHealthReduced;
        _health = 10;
    }

    public void ReduceHealth(float damage)
    {
        _health = damage > _health ? 0 : _health -= damage;

        OnDamaged?.Invoke(_health);
    }

    private void OnHealthReduced(float currentHP)
    {
        Debug.Log(currentHP);


        if(currentHP == 0)
        {
            OnDestroyed?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
