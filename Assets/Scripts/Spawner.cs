using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemy _prefab;
    [SerializeField] private List<Vector3> _startPoints;
    [SerializeField] private ZonaLifeEnemy _zonaLifeEnemy;
    [SerializeField] private float _repeatRate = 2f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

    private ObjectPool<Enemy> _pool;
    private WaitForSeconds _wait;
    private Coroutine _coroutine;
    private Vector3 _target = new Vector3(-19, 1, 19);

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

        enemy.Exited -= ReleaseEnemy;
    }

    private void ActionOnGet(Enemy enemy)
    {
        enemy.transform.position = _startPoints[UnityEngine.Random.Range(0, _startPoints.Count)];
        enemy.transform.LookAt(_target);
        enemy.gameObject.SetActive(true);

        enemy.Exited += ReleaseEnemy;
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