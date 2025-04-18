using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Movement : MonoBehaviour
{
    private int _startSpeed = 1;
    private int _maxSpeed = 10;
    private int _currentSpeed;
    private int _stepSpeed = 1;

    private float _delay = 0.20f;
    private WaitForSeconds _wait;
    private Animator _animator;
    private Coroutine _coroutine;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _wait = new WaitForSeconds(_delay);
        _currentSpeed = _startSpeed;
    }

    private void OnEnable()
    {
        _coroutine = StartCoroutine(CalculateSpeed());
    }
    private void OnDisable()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private void Update()
    {
        Move();
    }

    public void ResetSpeed ()
    {
        _currentSpeed = _startSpeed;
    }

    private void Move()
    {
        transform.Translate(_currentSpeed * Time.deltaTime * Vector3.forward);
        _animator.SetFloat("Speed", _currentSpeed);
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
