using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Movement))]
public class Enemy : MonoBehaviour
{
    public Rigidbody _rigidbody;

    public Movement Movement { get; private set; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Movement = GetComponent<Movement>();

        _rigidbody.useGravity = true;
    }
}
