using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Movement : MonoBehaviour
{
    private int _startSpeed = 1;
    private int _maxSpeed = 10;
    private int _currentSpeed;
    private int _stepSpeed = 1;

    private List<Transform> _waypoints = new();
    private int _startWaypoint = 0;
    private int _currentWaypoint;
    private float _delay = 0.20f;
    private WaitForSeconds _wait;
    private Animator _animator;
    private Coroutine _coroutine;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _wait = new WaitForSeconds(_delay);
        _currentSpeed = _startSpeed;
        _currentWaypoint = _startWaypoint;
    }

    private void OnEnable()
    {
        _coroutine = StartCoroutine(CalculateSpeed());
    }

    private void OnDisable()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }

    private void Update()
    {
        if (_waypoints.Count != 0)
        {
            if (transform.position == _waypoints[_currentWaypoint].position)
            {
                _currentWaypoint = (_currentWaypoint + 1) % _waypoints.Count;
            }

            transform.LookAt(_waypoints[_currentWaypoint]);
            transform.position = Vector3.MoveTowards(transform.position, _waypoints[_currentWaypoint].position, _currentSpeed * Time.deltaTime);

            _animator.SetFloat(PlayerAnimatorData.Params.Speed, _currentSpeed);
        }
    }

    public void Reset()
    {
        _currentWaypoint = _startWaypoint;
        _currentSpeed = _startSpeed;
    }

    public void TakeRoute(List<Transform> waypoints)
    {
        _waypoints = waypoints;
    }

    private IEnumerator CalculateSpeed()
    {
        for (int i = _startSpeed; i < _maxSpeed; i++)
        {
            yield return _wait;
            _currentSpeed += _stepSpeed;
        }
    }
}

public static class PlayerAnimatorData
{
    public static class Params
    {
        public static readonly int Speed = Animator.StringToHash(nameof(Speed));
    }
}