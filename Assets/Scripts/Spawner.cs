using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemy _prefab;
    [SerializeField] private PointSpawn _startPoint;
    [SerializeField] private ZonaLifeEnemy _zonaLifeEnemy;
    [SerializeField] private float _repeatRate = 2f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

    private ObjectPool<Enemy> _pool;
    private WaitForSeconds _wait;
    private Coroutine _coroutine;

    private List<Vector3> _targets = new();

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

        _targets.Add(new Vector3(0, 1, 10));
        _targets.Add(new Vector3(10, 1, 0));
        _targets.Add(new Vector3(10, 1, 10));
        _targets.Add(new Vector3(0, 1, -10));
        _targets.Add(new Vector3(-10, 1, 0));
        _targets.Add(new Vector3(-10, 1, -10));
    }

    private void OnEnable()
    {
        _coroutine = StartCoroutine(GetEnemyOverTime());
    }

    private void OnDisable()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }

    private void ActionOnRelease(Enemy enemy)
    {
        enemy.Movement.ResetSpeed();
        enemy.gameObject.SetActive(false);

        _zonaLifeEnemy.Exited -= ReleaseEnemy;
    }

    private void ActionOnGet(Enemy enemy)
    {
        int numberTarget = UnityEngine.Random.Range(0, _targets.Count);

        enemy.transform.position = _startPoint.transform.position;
        enemy.transform.LookAt(_targets[numberTarget]);
        enemy.gameObject.SetActive(true);

        _zonaLifeEnemy.Exited += ReleaseEnemy;
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