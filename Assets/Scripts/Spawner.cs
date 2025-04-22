using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    private float _repeatRate = 2f;
    private int _poolCapacity = 5;
    private int _poolMaxSize = 5;

    private ObjectPool<Enemy> _pool;
    private WaitForSeconds _wait;
    private Coroutine _coroutine;
    private Enemy _prefab;
    private Transform _startPoint;
    private List<Transform> _waipoints;

    private void Awake()
    {
        _pool = new ObjectPool<Enemy>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (enemy) => ActionOnGet(enemy),
            actionOnRelease: (enemy) => ActionOnRelease(enemy),
            actionOnDestroy: (enemy) => Destroy(enemy),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);

        _wait = new WaitForSeconds(_repeatRate);
    }

    private void OnDisable()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }

    public void Work(Enemy prefab, Transform startPoint, List<Transform> waipoints)
    {
        _prefab = prefab;
        _startPoint = startPoint;
        _waipoints = waipoints;
        _coroutine = StartCoroutine(GetEnemyOverTime());
    }

    private void ActionOnRelease(Enemy enemy)
    {
        enemy.Movement.Reset();
        enemy.gameObject.SetActive(false);

        enemy.ExitedZonaLife -= ReleaseEnemy;
        enemy.EnteredTargetArea -= ReleaseEnemy;
    }

    private void ActionOnGet(Enemy enemy)
    {
        enemy.transform.position = _startPoint.position;
        enemy.gameObject.SetActive(true);

        if (_waipoints.Count != 0)
        {
            enemy.Movement.TakeRoute(_waipoints);
        }

        enemy.ExitedZonaLife += ReleaseEnemy;
        enemy.EnteredTargetArea += ReleaseEnemy;
    }

    private void GetEnemy()
    {
        _pool.Get();
    }

    private void ReleaseEnemy(Enemy enemy)
    {
        _pool.Release(enemy);
    }

    private IEnumerator GetEnemyOverTime()
    {
        while (gameObject.activeSelf)
        {
            yield return _wait;
            GetEnemy();
        }
    }
}