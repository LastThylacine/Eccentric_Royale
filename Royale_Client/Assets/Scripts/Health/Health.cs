using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action<float> UpdateHealth;

    [field: SerializeField] public float Max { get; private set; } = 10f;
    private float _current;

    private void Start()
    {
        _current = Max;
    }

    public void ApplyDamage(float value)
    {
        _current -= value;
        if (_current < 0) _current = 0;

        UpdateHealth?.Invoke(_current);
    }
}

public interface IHealth
{
    Health Health { get; }
}