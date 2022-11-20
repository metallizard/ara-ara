using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Venue : MonoBehaviour
{
    public delegate void VenueSpawned(Venue venue);
    public static event VenueSpawned OnVenueSpawned;

    public delegate void Destroyed(Venue venue);
    public event Destroyed OnDestroyed;

    private delegate void Damaged(float currentHP);
    private event Damaged OnDamaged;

    [SerializeField]
    private Slider _healthBar;

    private float _health;

    private float _maxHealth;

    private bool _isDead;

    private void Awake()
    {
        OnDamaged += OnHealthReduced;
        _maxHealth = _health = 10;

        OnVenueSpawned?.Invoke(this);
    }

    public void ReduceHealth(float damage)
    {
        _health = damage > _health ? 0 : _health -= damage;

        OnDamaged?.Invoke(_health);
    }

    private void OnHealthReduced(float currentHP)
    {
        _healthBar.value = _health / _maxHealth;

        if(currentHP == 0 && !_isDead)
        {
            _isDead = true;
            OnDestroyed?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}
