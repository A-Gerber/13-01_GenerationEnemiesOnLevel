using System.Collections.Generic;
using UnityEngine;

public class HandlerEnemies : MonoBehaviour
{
    [SerializeField] private Spawner _prefabSpawner;
    [SerializeField] private Enemy[] _enemies;
    [SerializeField] private Transform[] _startPoints;
    [SerializeField] List<Transform> _route1;
    [SerializeField] List<Transform> _route2;
    [SerializeField] List<Transform> _route3;

    private List<Spawner> _spawners = new();
    private List<List<Transform>> _routes = new();
    private int _countSpawners = 3;

    private void Awake()
    {
        Fill();
    }

    private void Start()
    {
        for (int i = 0; i < _countSpawners; i++)
        {
            _spawners[i].Work(_enemies[i], _startPoints[i], _routes[i]);
        }
    }

    private void Fill()
    {
        _routes.Add(_route1);
        _routes.Add(_route2);
        _routes.Add(_route3);

        for (int i = 0; i < _countSpawners; i++)
        {          
            _spawners.Add(Instantiate(_prefabSpawner));
        }
    }
}