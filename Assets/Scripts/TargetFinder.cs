using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFinder : MonoBehaviour
{
    private Venue[] _venues;

    private void Awake()
    {
        _venues = FindObjectsOfType<Venue>();

        Debug.Log(_venues.Length);
    }

    public Venue GetTarget()
    {
        return _venues[0];
    }
}
