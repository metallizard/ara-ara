using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    private Venue _target;

    private bool _beingPushed;

    public float Speed;

    public void SetTarget(Venue venue)
    {
        _target = venue;
        Speed = Random.Range(0.05f, 0.25f);
    }

    public void Push(Vector3 direction, float force)
    {
        _beingPushed = true;
        StartCoroutine(PushHelper(direction, force));
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
