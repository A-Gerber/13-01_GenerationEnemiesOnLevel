using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Target : MonoBehaviour
{
    [SerializeField] private List<Transform> _waypoints;

    private Movement _movement;

    private void Awake()
    {
        _movement = GetComponent<Movement>();
    }

    private void Start()
    {
        _movement.TakeRoute(_waypoints);
    }
}