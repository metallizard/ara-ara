using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameContext : MonoBehaviour
{
    public delegate void TimerTick(float t);
    public static event TimerTick OnTimerTick;

    public delegate void VenueListChanged(List<Venue> venues);
    public static event VenueListChanged OnVenueListChanged;

    public delegate void GameOverDelegate();
    public static event GameOverDelegate OnGameOver;

    public delegate void GameWinDelegate();
    public static event GameWinDelegate OnGameWin;

    [SerializeField]
    private AudioManager _audioManager;

    [SerializeField]
    private GameObject _prefabCloud;

    [SerializeField]
    private GameObject _prefabThunderCloud;

    [SerializeField]
    private TargetFinder _targetFinder;

    [SerializeField]
    private float _gameTime;

    private Dictionary<Cloud, Venue> _cloudOnVenue = new Dictionary<Cloud, Venue>();

    private List<Cloud> _clouds = new List<Cloud>();

    private List<Venue> _venues = new List<Venue>();

    private IEnumerator _timerLoop;

    private int _cloudVisibleCount = 0;

    private bool _isGameover = false;

    private void Awake()
    {
        OnTimerTick += GameContext_OnTimerTick;
        Venue.OnVenueSpawned += OnVenueSpawned;
        Player.OnDead += Player_OnDead;

        Time.timeScale = 1;
    }

    private void Start()
    {
        StartCoroutine(StartGame());
    }

    // Start is called before the first frame update
    IEnumerator StartGame()
    {
        _audioManager.PlayBGM();

        _timerLoop = TimerLoop();
        StartCoroutine(_timerLoop);

        float count = 30;
        while(count > 0)
        {
            SpawnCloud();
            yield return new WaitForSeconds(3f);
            count--;
        }
    }

    private void GameOver()
    {
        _isGameover = true;
        _audioManager.OrchestrateGameOverSFX();

        Time.timeScale = 0;
        OnGameOver?.Invoke();
    }

    private void GameWin()
    {
        OnGameWin?.Invoke();
    }

    private IEnumerator TimerLoop()
    {
        while (_gameTime > 0)
        {
            OnTimerTick?.Invoke(_gameTime);
            _gameTime--;
            yield return new WaitForSeconds(1);
        }

        Time.timeScale = 0;
        GameWin();
    }

    private void SpawnCloud()
    {
        var spawnThunder = _clouds.Count % 5 == 0;
        GameObject cloudObject;
        if (spawnThunder)
        {
            cloudObject = Instantiate<GameObject>(_prefabThunderCloud, GetSpawnPosition(), Quaternion.identity);

        }
        else
        {
            cloudObject = Instantiate<GameObject>(_prefabCloud, GetSpawnPosition(), Quaternion.identity);
        }

        Cloud cloud = cloudObject.GetComponent<Cloud>();

        cloud.SetTarget(_targetFinder.GetTarget());
        cloud.OnDestroyed += Cloud_OnDestroyed;
        cloud.OnLeaveVenue += Cloud_OnLeaveVenue;
        cloud.OnReachVenue += Cloud_OnReachVenue;
        cloud.OnTargetDestroyed += Cloud_OnTargetDestroyed;
        cloud.OnVisibleToCamera += Cloud_OnVisibleToCamera;
        cloud.OnLeaveCamera += Cloud_OnLeaveCamera;

        _clouds.Add(cloud);
    }  

    private void DistributeDamage()
    {
        var affected = _cloudOnVenue.Values.ToArray();

        for(int i = 0; i < affected.Length; i++)
        {
            affected[i].ReduceHealth(1);
        }
    }

    #region Event Listener
    private void Player_OnDead()
    {
        if(!_isGameover) GameOver();
    }

    private void Cloud_OnLeaveCamera(Cloud cloud)
    {
        _cloudVisibleCount -= 1;
        if (_cloudVisibleCount <= 0)
        {
            _cloudVisibleCount = 0;
            _audioManager?.StopRain();
        }
    }

    private void Cloud_OnVisibleToCamera(Cloud cloud)
    {
        _cloudVisibleCount++;

        _audioManager?.PlayRain();
    }

    private void OnVenueSpawned(Venue venue)
    {
        if(_venues.Contains(venue))
        {
            Debug.LogError("lah udah ada venuenya cok");
            return;
        }

        _venues.Add(venue);
        OnVenueListChanged?.Invoke(_venues);

        venue.OnDestroyed += OnVenueDestroyed;
    }

    private void OnVenueDestroyed(Venue venue)
    {
        if (!_venues.Contains(venue))
        {
            Debug.LogError("lah ga ada venue yang bisa didestroy cok");
            return;
        }

        _venues.Remove(venue);
        OnVenueListChanged?.Invoke(_venues);

        if (_venues.Count == 0)
        {
            if(!_isGameover) GameOver();
        }
    }

    private void GameContext_OnTimerTick(float t)
    {
        DistributeDamage();

        if(t == 5)
        {
            _audioManager.Play5SecLeft();
        }
    }

    private void Cloud_OnReachVenue(Cloud cloud, Venue venue)
    {
        _cloudOnVenue.Add(cloud, venue);
    }

    private void Cloud_OnLeaveVenue(Cloud cloud)
    {
        _cloudOnVenue.Remove(cloud);
    }

    private void Cloud_OnTargetDestroyed(Cloud cloud, Venue venue)
    {
        var newTarget = _targetFinder.GetTarget();
        if(newTarget)
        {
            cloud.SetTarget(newTarget);
        }
    }

    private void Cloud_OnDestroyed(Cloud cloud)
    {
        if(_cloudOnVenue.ContainsKey(cloud))
        {
            _cloudOnVenue.Remove(cloud);
        }

        if(_clouds.Contains(cloud))
        {
            _clouds.Remove(cloud);
        }
    }
    #endregion

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

    private void OnDestroy()
    {
        foreach (var cloud in _clouds)
        {
            cloud.OnDestroyed -= Cloud_OnDestroyed;
            cloud.OnLeaveVenue -= Cloud_OnLeaveVenue;
            cloud.OnReachVenue -= Cloud_OnReachVenue;
            cloud.OnTargetDestroyed -= Cloud_OnTargetDestroyed;
            cloud.OnVisibleToCamera -= Cloud_OnVisibleToCamera;
            cloud.OnLeaveCamera -= Cloud_OnLeaveCamera;
        }

        OnTimerTick -= GameContext_OnTimerTick;
    }
}
