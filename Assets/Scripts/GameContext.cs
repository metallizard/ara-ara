using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameContext : MonoBehaviour
{
    public delegate void TimerTick(float t);
    public static event TimerTick OnTimerTick;

    [SerializeField]
    private GameObject _prefabCloud;

    [SerializeField]
    private TargetFinder _targetFinder;

    [SerializeField]
    private float _gameTime;

    private Dictionary<Cloud, Venue> _cloudOnVenue = new Dictionary<Cloud, Venue>();

    private IEnumerator _timerLoop;

    private void Awake()
    {
        OnTimerTick += GameContext_OnTimerTick;
    }

    private void Start()
    {
        StartCoroutine(StartGame());
    }

    // Start is called before the first frame update
    IEnumerator StartGame()
    {
        _timerLoop = TimerLoop();
        StartCoroutine(_timerLoop);

        float count = 5;
        while(count > 0)
        {
            SpawnCloud();
            yield return new WaitForSeconds(0.5f);
            count--;
        }
    }

    private IEnumerator TimerLoop()
    {
        while (_gameTime > 0)
        {
            OnTimerTick?.Invoke(_gameTime);
            _gameTime--;
            yield return new WaitForSeconds(1);
        }
    }

    private void SpawnCloud()
    {
        GameObject cloudObject = Instantiate<GameObject>(_prefabCloud, GetSpawnPosition(), Quaternion.identity);
        Cloud cloud = cloudObject.GetComponent<Cloud>();

        cloud.SetTarget(_targetFinder.GetTarget());
        cloud.OnDestroyed += Cloud_OnDestroyed;
        cloud.OnLeaveVenue += Cloud_OnLeaveVenue;
        cloud.OnReachVenue += Cloud_OnReachVenue;
    }

    private void DistributeDamage()
    {
        var affected = _cloudOnVenue.Values.ToArray();

        for(int i = 0; i < affected.Length; i++)
        {
            affected[i].ReduceHealth(1);
        }
    }

    private void GameContext_OnTimerTick(float t)
    {
        DistributeDamage();
    }

    private void Cloud_OnReachVenue(Cloud cloud, Venue venue)
    {
        _cloudOnVenue.Add(cloud, venue);
    }

    private void Cloud_OnLeaveVenue(Cloud cloud)
    {
        _cloudOnVenue.Remove(cloud);
    }

    private void Cloud_OnDestroyed(Cloud cloud)
    {
        if(_cloudOnVenue.ContainsKey(cloud))
        {
            _cloudOnVenue.Remove(cloud);
        }
    }

    private Vector3 GetSpawnPosition()
    {
        float spawnOffset = 1.1f;
        bool randomizeHorizontal = Random.Range(0.0f, 1.0f) > 0.5f;
        float clampedAxis = Random.Range(0.0f, 1.0f) > 0.5f ? spawnOffset : 1 - spawnOffset;

        if(randomizeHorizontal)
        {
            return Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(1 - spawnOffset, spawnOffset), clampedAxis, 0));
        }
        else
        {
            return Camera.main.ViewportToWorldPoint(new Vector3(clampedAxis, Random.Range(1 - spawnOffset, spawnOffset), 0));
        }
    }
}
