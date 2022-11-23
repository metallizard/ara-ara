using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    public float Speed;

    public void Shoot(Vector3 target)
    {
        StartCoroutine(ShootHelper(target));

        Invoke("SelfDestroy", 10);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>())
        {
            collision.GetComponent<Player>().Stun();
        }
    }

    private IEnumerator ShootHelper(Vector3 target)
    {
        while(true)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, Speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, target) < 0.1) SelfDestroy();
            yield return null;
        }
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
