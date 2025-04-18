using System;
using System.Collections;
using UnityEngine;

public class ZonaLifeEnemy : MonoBehaviour
{
    private WaitForSeconds _wait;
    private float _lifetime = 2.0f;

    public event Action<Enemy> Exited;

    private void OnTriggerExit(Collider other)
    {       
        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            _wait = new WaitForSeconds(_lifetime);

            StartCoroutine(DieOverTime(enemy));
        }
    }

    private IEnumerator DieOverTime(Enemy enemy)
    {
        yield return _wait;

        Exited?.Invoke(enemy);
    }
}
