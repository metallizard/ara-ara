using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void Dead();
    public static event Dead OnDead;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private PlayerInput _input;

    [SerializeField]
    private float _speed;

    [SerializeField]
    private bool _alwaysRepel;

    private bool _isStunned = false;

    private void Start()
    {
        _input.OnValueChanged += Move;
    }

    public void Move(Vector3 direction)
    {
        if (_isStunned) return;

        var clampedDirection = GetClampedDirection(direction);

        transform.Translate(clampedDirection * _speed * Time.deltaTime);

        float h = Mathf.Abs(direction.x) > Mathf.Abs(direction.y) ? direction.x : 0;
        float v = Mathf.Abs(direction.y) > Mathf.Abs(direction.x) ? direction.y : 0;

        _animator.SetFloat("Vertical", v);
        _animator.SetFloat("Horizontal", h);
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

    public void Stun()
    {
        if (_isStunned) return;
        _isStunned = true;
        StartCoroutine(Blink(2));
        Invoke("Unstun", 2);
    }

    private void Unstun()
    {
        _isStunned = false;
    }

    private IEnumerator Blink(float duration)
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color col = renderer.color;
        float blinkRate = 0.1f;
        while(duration > 0)
        {
            col.a = col.a == 1 ? 0 : 1;
            renderer.color = col;
            duration -= blinkRate;
            yield return new WaitForSeconds(blinkRate);
        }

        col.a = 1;
        renderer.color = col;
    }

    private Vector3 GetClampedDirection(Vector3 direction)
    {
        Vector3 clamped = direction;

        if (transform.position.x + direction.x > Camera.main.ViewportToWorldPoint(new Vector3(0.95f, 0, 0)).x)
        {
            clamped.x = 0;
        }
        if (transform.position.x + direction.x < Camera.main.ViewportToWorldPoint(new Vector3(0.05f, 0, 0)).x)
        {
            clamped.x = 0;
        }
        if (transform.position.y + direction.y > Camera.main.ViewportToWorldPoint(new Vector3(0, 0.95f, 0)).y)
        {
            clamped.y = 0;
        }
        if (transform.position.y + direction.y < Camera.main.ViewportToWorldPoint(new Vector3(0, 0.05f, 0)).y)
        {
            clamped.y = 0;
        }

        return clamped;
    }
}
