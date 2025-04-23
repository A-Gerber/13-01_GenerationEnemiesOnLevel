using System.Collections.Generic;
using UnityEngine;

public class PointSpawn : MonoBehaviour
{
    [SerializeField] private Enemy _prefab;
    [SerializeField] private List <Transform> _targets;

    public Enemy CreateEnemy()
    {
        Enemy enemy = Instantiate(_prefab);
        enemy.Movement.TakeRoute(_targets);
        enemy.SetBirthplace(transform.position);

        return enemy;
    }
}