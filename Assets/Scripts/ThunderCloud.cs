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
        float t = Random.Range(2, 6);

        while(t > 0)
        {
            t -= Time.deltaTime;
            yield return null;
        }

        Shoot();
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
