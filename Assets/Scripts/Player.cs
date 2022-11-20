using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void Dead();
    public static event Dead OnDead;

    [SerializeField]
    private PlayerInput _input;

    [SerializeField]
    private float _speed;

    [SerializeField]
    private bool _alwaysRepel;

    private void Start()
    {
        _input.OnValueChanged += Move;
    }

    public void Move(Vector3 direction)
    {
        transform.Translate(direction * _speed * Time.deltaTime);
    }

    private void Update()
    {
        if(_alwaysRepel || Input.GetKeyDown(KeyCode.Space)) Repel();
    }

    public void Repel()
    {
        Collider2D[] overlappedCollider = Physics2D.OverlapCircleAll(transform.position, 3);
        foreach(var col in overlappedCollider)
        {
            if (!col.GetComponentInParent<Cloud>()) continue;

            Vector3 dir = (col.transform.position - transform.position).normalized;
            col.GetComponentInParent<Cloud>().Push(dir, (6 - Vector3.Distance(transform.position, col.transform.position)) * 1);
        }
    }

    public void Kill()
    {
        OnDead?.Invoke();
    }
}
