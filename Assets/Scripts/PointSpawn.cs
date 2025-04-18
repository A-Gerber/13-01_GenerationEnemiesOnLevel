using System.Collections;
using UnityEngine;

public class PointSpawn : MonoBehaviour
{
    private int _positionY = 2;
    private float _minHorizontalPosition = -2.5f;
    private float _maxHorizontalPosition = 2.5f;

    private float _repeatRate = 2.0f;
    private WaitForSeconds _wait;
    private Coroutine _coroutine;

    private void Awake()
    {
        _wait = new WaitForSeconds(_repeatRate);
    }

    private void OnEnable()
    {
        _coroutine = StartCoroutine(ChangePositionOverTime());
    }

    private void OnDisable()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }

    private void ChangePosition()
    {     
        transform.position =  new Vector3(
            UnityEngine.Random.Range(_minHorizontalPosition, _maxHorizontalPosition + 1),
            _positionY,
            UnityEngine.Random.Range(_minHorizontalPosition, _maxHorizontalPosition + 1));
    }

    private IEnumerator ChangePositionOverTime()
    {
        while (gameObject.activeSelf)
        {
            yield return _wait;
            ChangePosition();
        }
    }
}