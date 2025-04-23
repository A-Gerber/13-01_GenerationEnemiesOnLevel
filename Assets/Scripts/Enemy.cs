using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Movement))]
public class Enemy : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private WaitForSeconds _wait;
    private float _lifetime = 2.0f;

    public event Action<Enemy> ExitedZonaLife;
    public event Action<Enemy> ComedToTarget;

    public Movement Movement { get; private set; }
    public Vector3 Birthplace { get; private set; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Movement = GetComponent<Movement>();

        _rigidbody.useGravity = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<ZonaLifeEnemy>(out _))
        {
            _wait = new WaitForSeconds(_lifetime);

            StartCoroutine(DieOverTime());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Target>(out _))
        {
            ComedToTarget?.Invoke(this);
        }
    }

    public void SetBirthplace(Vector3 position)
    {
        Birthplace = position;
    }

    private IEnumerator DieOverTime()
    {
        yield return _wait;

        ExitedZonaLife?.Invoke(this);
    }
}