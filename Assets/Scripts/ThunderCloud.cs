using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderCloud : Cloud
{
    private IEnumerator _shootProcedure;

    [SerializeField]
    private GameObject _thunderPrefab;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnBecameInvisible()
    {
        base.OnBecameInvisible();

        StopCoroutine(_shootProcedure);
    }

    protected override void OnBecameVisible()
    {
        base.OnBecameVisible();

        _shootProcedure = ShootProcedure();
        StartCoroutine(_shootProcedure);
    }

    private IEnumerator ShootProcedure()
    {
        float t = Random.Range(8, 11);

        while(t > 0)
        {
            t -= 1;
            if (t == 1) StartCoroutine(Blink());
            yield return new WaitForSeconds(1);
        }

        Shoot();
    }

    private IEnumerator Blink()
    {
        float duration = 1;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color col = renderer.color;
        float blinkRate = 0.25f;
        while (duration > 0)
        {
            col.a = col.a == 1 ? 0 : 1;
            renderer.color = col;
            duration -= blinkRate;
            yield return new WaitForSeconds(blinkRate);
        }

        col.a = 1;
        renderer.color = col;
    }

    private void Shoot()
    {
        Vector3 target = GameObject.FindWithTag("Player").transform.position;

        Thunder thunder = Instantiate(_thunderPrefab, transform.position, Quaternion.identity).GetComponent<Thunder>();
        thunder.Shoot(target);

        _shootProcedure = ShootProcedure();
        StartCoroutine(_shootProcedure);
    }
}
