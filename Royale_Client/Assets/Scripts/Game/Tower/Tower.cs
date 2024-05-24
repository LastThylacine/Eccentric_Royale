using Mirror;
using System;
using UnityEngine;

[RequireComponent(typeof(Health), typeof(NetworkIdentity), typeof(NetworkTransformUnreliable))]
public class Tower : NetworkBehaviour, IHealth,IDestroyed
{
    public event Action Destroyed;

    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public float Radius { get; private set; } = 2f;

    public float GetDistance(in Vector3 point) => Vector3.Distance(transform.position, point) - Radius;

    private void Start()
    {
        Health.UpdateHealth += CheckDestroy;
    }

    private void CheckDestroy(float currentHealth)
    {
        if (currentHealth > 0) return;

        Health.UpdateHealth -= CheckDestroy;
        Destroy(gameObject);

        Destroyed?.Invoke();
    }
}
