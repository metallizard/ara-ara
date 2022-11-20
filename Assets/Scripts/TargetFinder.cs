using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFinder : MonoBehaviour
{
    private List<Venue> _venues = new List<Venue>();

    private void Awake()
    {
        GameContext.OnVenueListChanged += OnVenueListChanged;
    }

    private void OnVenueListChanged(List<Venue> venues)
    {
        _venues = venues;
    }

    public Venue GetTarget()
    {
        if (_venues.Count == 0) return null;

        return _venues[Random.Range(0, _venues.Count)];
    }
}
