using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public delegate void ReachVenue(Cloud cloud, Venue venue);
    public event ReachVenue OnReachVenue;

    public delegate void LeaveVenue(Cloud cloud);
    public event LeaveVenue OnLeaveVenue;

    public delegate void Destroyed(Cloud cloud);
    public event Destroyed OnDestroyed;

    private Venue _target;

    private bool _beingPushed;

    public float Speed;

    public float TimeToLive;

    public void SetTarget(Venue venue)
    {
        _target = venue;
        Speed = Random.Range(1.5f, 2.25f);
        TimeToLive = 10;
        Invoke("Destroy", TimeToLive);
    }

    public void Push(Vector3 direction, float force)
    {
        _beingPushed = true;
        StartCoroutine(PushHelper(direction, force));
    }

    public void Destroy()
    {
        OnDestroyed?.Invoke(this);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Venue>())
        {
            OnReachVenue?.Invoke(this, collision.GetComponent<Venue>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Venue>())
        {
            OnLeaveVenue?.Invoke(this);
        }
    }

    void Update()
    {
        if (_target && !_beingPushed) MoveToTarget();
    }

    private void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, _target.transform.position, Speed * Time.deltaTime);
    }

    private IEnumerator PushHelper(Vector3 direction, float force)
    {
        while (force > 0)
        {
            transform.position += direction * force * Time.deltaTime;
            force--;
            yield return null;
        }

        _beingPushed = false;
    }
}
