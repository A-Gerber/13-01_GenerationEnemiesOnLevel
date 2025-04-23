using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private PointSpawn[] _pointsSpawn;
    [SerializeField] private float _repeatRate = 2.0f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

    private ObjectPool<Enemy> _pool;
    private WaitForSeconds _wait;
    private Coroutine _coroutine;
    private int _queueNumber = 0;

    private void Awake()
    {
        _pool = new ObjectPool<Enemy>(
            createFunc: () => CreateEnemy(),
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

    private Enemy CreateEnemy()
    {
        Enemy enemy = _pointsSpawn[_queueNumber % _pointsSpawn.Length].CreateEnemy();
        _queueNumber++;

        return enemy;
    }

    private void ActionOnRelease(Enemy enemy)
    {
        enemy.Movement.Reset();
        enemy.gameObject.SetActive(false);

        enemy.ExitedZonaLife -= ReleaseEnemy;
        enemy.ComedToTarget -= ReleaseEnemy;
    }

    private void ActionOnGet(Enemy enemy)
    {
        enemy.transform.position = enemy.Birthplace;
        enemy.gameObject.SetActive(true);

        enemy.ExitedZonaLife += ReleaseEnemy;
        enemy.ComedToTarget += ReleaseEnemy;
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