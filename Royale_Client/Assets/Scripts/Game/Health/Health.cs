using System;
using System.Collections;
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

    public void ApplyDelayDamage(float delay, float damage)
    {
        StartCoroutine(DelayDamage(delay, damage));
    }

    private IEnumerator DelayDamage(float delay, float damage)
    {
        yield return new WaitForSeconds(delay);
        ApplyDamage(damage);
    }
}

public interface IHealth
{
    Health Health { get; }
}