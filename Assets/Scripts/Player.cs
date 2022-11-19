using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _input;

    [SerializeField]
    private float _speed;

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
        //if(Input.GetKeyDown(KeyCode.Space))
        Repel();
    }

    public void Repel()
    {
        Collider2D[] overlappedCollider = Physics2D.OverlapCircleAll(transform.position, 3);
        foreach(var col in overlappedCollider)
        {
            Vector3 dir = (col.transform.position - transform.position).normalized;
            col.GetComponent<Cloud>().Push(dir, (6 - Vector3.Distance(transform.position, col.transform.position)) * 10);
        }
    }
}
