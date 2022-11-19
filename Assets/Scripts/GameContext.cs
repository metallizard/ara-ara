using System.Collections;
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

    private IEnumerator _timerLoop;

    private void Start()
    {
        StartCoroutine(StartGame());
    }

    // Start is called before the first frame update
    IEnumerator StartGame()
    {
        _timerLoop = TimerLoop();
        StartCoroutine(_timerLoop);

        float count = 20;
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
        GameObject cloud = Instantiate<GameObject>(_prefabCloud, GetSpawnPosition(), Quaternion.identity);
        cloud.GetComponent<Cloud>().SetTarget(_targetFinder.GetTarget());
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
